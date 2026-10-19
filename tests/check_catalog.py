#!/usr/bin/env python3
"""Fast offline guard against mis-mapped animal variants and missing textures."""
import json
import re
from pathlib import Path

root = Path(__file__).resolve().parents[1]
source = (root / "src/Plugin.cs").read_text(encoding="utf-8")
materials = json.loads((root / "assets/MATERIAL_TEXTURE_MAP.json").read_text(encoding="utf-8"))
pattern = r'\{\s*"(\w+)",\s*new\[\]\s*\{([^}]+)\}\s*\}'
variants = {
    match.group(1): re.findall(r'"([^"]+)"', match.group(2))
    for match in re.finditer(pattern, source)
}
assert len(variants) == 9, variants
assert sum(map(len, variants.values())) == 47, variants
assert len(materials) == 59, len(materials)

errors = []
for animal, choices in variants.items():
    bundle_folder = "Drake" if animal == "Hatchling" else "Askvin" if animal == "Asksvin" else animal
    for variant in choices:
        prefix = f"{bundle_folder}/{variant}/".lower()
        group = [item for item in materials if item.get("asset_path", "").lower().startswith(prefix)]
        expected = 3 if animal == "Lox" else 1
        if len(group) != expected:
            errors.append(f"{animal}/{variant}: expected {expected} materials, found {len(group)}")
        for material in group:
            main = material["textures"].get("_MainTex", {})
            filepath = main.get("file")
            if not filepath or not (root / "assets" / filepath).is_file():
                errors.append(f"{animal}/{variant}: missing main texture {filepath!r}")

if errors:
    raise SystemExit("Catalog mismatch:\n" + "\n".join(errors))

assert "new Material(" not in source, "Found per-mob material allocation"
assert "HarmonyPatch(typeof(LevelEffects)" not in source, "Global level patch returned"
assert source.count("ToneDownAlbinoDeer(material);") == 1
assert 'variantName.Equals("Albino", StringComparison.OrdinalIgnoreCase)' in source
assert 'animal.Equals("Deer", StringComparison.OrdinalIgnoreCase)' in source
correction = source.split("private static void ToneDownAlbinoDeer(Material material)",1)[1].split("internal static bool TryApply",1)[0]
for property_name in ("_Color", "_Glossiness", "_GlossMapScale", "_Metallic", "_SpecularHighlights", "_GlossyReflections", "_EmissionColor"):
    assert property_name in correction, property_name
assert "new Material(" not in correction
print("PASS: 47 variants, 59 materials and albino deer glare correction.")
