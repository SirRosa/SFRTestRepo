#!/usr/bin/env python3
"""Build a searchable map from animal/skin/material to editable PNG texture."""
from __future__ import annotations

import json
import re
import sys
from pathlib import Path

def main() -> None:
    root = Path(sys.argv[1] if len(sys.argv) > 1 else "recovered/assets")
    manifest = json.loads((root / "MANIFEST.json").read_text(encoding="utf-8"))
    records = manifest["objects"]
    by_id = {o["path_id"]: o for o in records}
    materials = []
    for obj in records:
        if obj["type"] != "Material" or not obj.get("file"):
            continue
        tree = json.loads((root / obj["file"]).read_text(encoding="utf-8"))
        textures = {}
        saved = tree.get("m_SavedProperties", {})
        for pair in saved.get("m_TexEnvs", []):
            if not isinstance(pair, list) or len(pair) < 2:
                continue
            key, info = pair[0], pair[1]
            if not isinstance(info, dict):
                continue
            pointer = info.get("m_Texture") or {}
            texture_id = pointer.get("m_PathID")
            texture = by_id.get(texture_id, {})
            if texture_id in (None, 0):
                continue
            textures[key] = {
                "path_id": texture_id,
                "file": texture.get("file"),
                "name": texture.get("name"),
                "asset_path": texture.get("asset_path"),
            }
        materials.append({
            "name": obj.get("name"),
            "path_id": obj["path_id"],
            "asset_path": obj.get("asset_path"),
            "json_file": obj["file"],
            "textures": textures,
        })

    # This mapping is lossless with respect to Unity material Texture2D PPtrs.
    (root / "MATERIAL_TEXTURE_MAP.json").write_text(
        json.dumps(materials, indent=2, ensure_ascii=False), encoding="utf-8"
    )

    # Pull creature and skin names directly from the decompiled material loader.
    loader = root.parent / "source" / "BalrondFurRandomizer" / "ModResourceLoader.cs"
    declared = []
    if loader.exists():
        text = loader.read_text(encoding="utf-8")
        declared = re.findall(r'LoadAsset<Material>\((?:mainPath|text)\s*\+\s*"([^"]+)"\)', text)
    material_paths = {
        (m.get("asset_path") or "").replace("\\", "/").lower(): m
        for m in materials if m.get("asset_path")
    }

    lines = [
        "# Material → Texture2D catalogue",
        "",
        "Filenames and pointers recovered from the original DLL's Unity AssetBundle.",
        "Different materials may refer to the same texture. Source names like neck1",
        "are not necessarily unique, so always use material/texture IDs for exact lookup.",
        "",
        "## Variants loaded by the game code",
        "",
        "| Creature | Variant | Material in Unity | Main texture | Other texture slots |",
        "|---|---|---|---|---|",
    ]
    for path in declared:
        pieces = path.split("/")
        creature = pieces[0] if len(pieces) > 0 else "?"
        variant = pieces[1] if len(pieces) > 1 else "?"
        relevant = [
            m for asset_path, m in material_paths.items()
            if asset_path.endswith(path.lower())
        ]
        if len(relevant) == 1:
            mat = relevant[0]
            main = mat["textures"].get("_MainTex", {}).get("file") or "—"
            others = ", ".join(
                f"{key}: {value.get('file') or 'missing'}"
                for key, value in mat["textures"].items()
                if key != "_MainTex"
            )
            pathlink = "../" + str((root / mat["json_file"]).relative_to(root.parent))
            mainlink = "../" + str((root / main).relative_to(root.parent)) if main != "—" else "—"
            lines.append(f"| {creature} | {variant} | [{path}]({pathlink}) | [{main}]({mainlink}) | {others or '—'} |")
        else:
            lines.append(
                f"| {creature} | {variant} | {path} | **not resolved** | "
                + ("No AssetBundle container path" if not relevant else "Ambiguous container paths")
                + " |"
            )
    lines += ["", "## Recovery summary", "",
              f"- Materials: {len(materials)}",
              f"- Texture2D objects: {manifest.get('texture_count')}",
              f"- Game material loader references: {len(declared)}",
              f"- Matched paths: {sum(bool(any((p.lower() == (m.get('asset_path') or '').replace(chr(92),'/').lower()[-len(p):]) for m in materials)) for p in declared)}",
              "", "See [MATERIAL_TEXTURE_MAP.json](MATERIAL_TEXTURE_MAP.json) for exact path-ID mapping.",
              ""]
    (root / "MATERIAL_CATALOG.md").write_text("\n".join(lines), encoding="utf-8")
    print("Catalogued", len(materials), "materials,", len(declared), "game loader references.")

if __name__ == "__main__":
    main()
