#!/usr/bin/env python3
"""Recover Unity embedded resources, Texture2D PNGs, and material property trees.

Input: BalrondFurRandomizer.dll (an unchanged user-provided binary, or
      the corresponding Thunderstore release).
Requires: pip install UnityPy Pillow

This is a data extraction tool, not an attempt to reconstruct the Unity
asset project's authoring files.
"""
from __future__ import annotations

import hashlib
import json
import re
import struct
import sys
import traceback
from collections import Counter
from pathlib import Path

def slug(value: str, fallback: str = "unnamed") -> str:
    value = re.sub(r'[\\/:*?"<>|\x00-\x1f]', "_", value or fallback)
    return value.strip(" .")[:120] or fallback

def recover_bundle(dll: Path, target: Path) -> dict:
    data = dll.read_bytes()
    # The IL manifest embeds a UnityFS image without changing its header.
    # Reading UnityFS fileSize ensures a precise extraction rather than
    # accidentally copying the rest of the PE file.
    sig = b"UnityFS\x00"
    positions = [m.start() for m in re.finditer(re.escape(sig), data)]
    if not positions:
        raise RuntimeError("No UnityFS signature in DLL. Inspect managed resources separately.")
    errors = []
    for offset in positions:
        try:
            at = offset + len(sig)
            version = struct.unpack_from(">I", data, at)[0]
            at += 4
            end = data.index(b"\x00", at)
            player_version = data[at:end].decode("utf-8")
            at = end + 1
            end = data.index(b"\x00", at)
            engine_version = data[at:end].decode("utf-8")
            at = end + 1
            length = struct.unpack_from(">Q", data, at)[0]
            if version not in range(4, 10) or not 64 <= length <= len(data) - offset:
                raise ValueError("Invalid UnityFS version / size")
            target.parent.mkdir(parents=True, exist_ok=True)
            payload = data[offset: offset + length]
            target.write_bytes(payload)
            return {
                "offset": offset,
                "unityfs_format": version,
                "player_version": player_version,
                "engine_version": engine_version,
                "bytes": len(payload),
                "sha256": hashlib.sha256(payload).hexdigest(),
            }
        except Exception as exc:
            errors.append(f"{offset}: {exc}")
    raise RuntimeError("UnityFS candidates could not be parsed: " + "; ".join(errors))

def main() -> None:
    if len(sys.argv) != 3:
        raise SystemExit("Usage: recover_assets.py input.dll output_dir")
    dll = Path(sys.argv[1]).resolve()
    output = Path(sys.argv[2]).resolve()
    output.mkdir(parents=True, exist_ok=True)
    manifest: dict = {
        "input": dll.name,
        "input_sha256": hashlib.sha256(dll.read_bytes()).hexdigest(),
        "input_bytes": dll.stat().st_size,
        "texture_files": [],
        "material_files": [],
        "errors": [],
        "objects": [],
    }
    bundle = output / "raw" / "BalrondFurRandomizer.furrandomizer"
    manifest["bundle"] = recover_bundle(dll, bundle)

    try:
        import UnityPy
        environment = UnityPy.load(str(bundle))
    except Exception as exc:
        manifest["errors"].append("UnityPy failed to open bundle: " + repr(exc))
        (output / "MANIFEST.json").write_text(json.dumps(manifest, indent=2), encoding="utf-8")
        raise

    texture_dir = output / "textures"
    material_dir = output / "materials"
    texture_dir.mkdir(exist_ok=True)
    material_dir.mkdir(exist_ok=True)
    type_counts = Counter()
    claimed_names: set[str] = set()

    for obj in environment.objects:
        typename = obj.type.name
        type_counts[typename] += 1
        path_id = obj.path_id
        record = {"path_id": path_id, "type": typename}
        manifest["objects"].append(record)
        if typename not in ("Texture2D", "Material", "Shader", "Sprite", "TextAsset"):
            continue
        try:
            asset = obj.read()
            name = slug(getattr(asset, "m_Name", ""), f"{typename}_{path_id}")
            record["name"] = name
            stem = name
            if (typename, stem.lower()) in claimed_names:
                stem = f"{name}__{path_id}"
            claimed_names.add((typename, stem.lower()))

            if typename == "Texture2D":
                image = asset.image
                if image is None:
                    raise RuntimeError("Texture2D has no decodable image")
                filename = f"textures/{stem}.png"
                image.save(output / filename, format="PNG")
                record.update({"file": filename, "width": image.width, "height": image.height, "mode": image.mode})
                manifest["texture_files"].append(filename)
            elif typename in ("Material", "Shader", "Sprite", "TextAsset"):
                filename = f"materials/{typename}_{stem}.json"
                try:
                    tree = obj.read_typetree()
                except Exception:
                    tree = {
                        "m_Name": getattr(asset, "m_Name", ""),
                        "type": typename,
                        "remark": "Typetree unavailable; see raw AssetBundle.",
                    }
                # UnityPy may produce paths and non-JSON primitive values.
                (output / filename).write_text(
                    json.dumps(tree, ensure_ascii=False, indent=2, default=str),
                    encoding="utf-8",
                )
                record["file"] = filename
                manifest["material_files"].append(filename)
        except Exception as exc:
            manifest["errors"].append(
                f"{typename} (path_id {path_id}): {exc}\n{traceback.format_exc(limit=2)}"
            )

    manifest["object_counts"] = dict(sorted(type_counts.items()))
    manifest["texture_count"] = len(manifest["texture_files"])
    manifest["material_count"] = len(manifest["material_files"])
    (output / "MANIFEST.json").write_text(
        json.dumps(manifest, indent=2, ensure_ascii=False), encoding="utf-8"
    )
    print(json.dumps({
        "bundle_size": manifest["bundle"]["bytes"],
        "textures": manifest["texture_count"],
        "materials_and_other": manifest["material_count"],
        "object_counts": manifest["object_counts"],
        "errors": len(manifest["errors"]),
    }, indent=2))
    if not manifest["texture_count"]:
        raise RuntimeError("No textures exported; inspect recovered/MANIFEST.json before relying on results")

if __name__ == "__main__":
    main()
