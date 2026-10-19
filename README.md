# Sirroh Fur Randomizer

Projekt odzyskania kodu i materiałów z `BalrondFurRandomizer.dll` w celu dalszego rozwijania moda dla Valheim.

## Status
- **Źródło:** Balrond Fur Randomizer 1.0.7 (Thunderstore). Oryginalna DLL przekazana przez użytkownika.
- **Odzysk:** dekompilacja ILSpy + ekstrakcja Unity AssetBundle + eksport Texture2D do PNG.
- **Dokładność:** zdekompilowany C# **nie jest** oryginalnym kodem źródłowym; komentarze, część nazw lokalnych i struktura projektu mogą zostać utracone.
- **Kompilacja/zgodność Valheim 1.0:** niezweryfikowana; wymaga późniejszego odtworzenia referencji do bibliotek Valheim, BepInEx, Unity oraz testów gry.
- **Prawa:** odtworzenie kodu i grafik nie nadaje automatycznie prawa do ich publicznej redystrybucji. Przed publikacją paczki sprawdź warunki autora.

### Struktura (po odzyskaniu)
```text
recovered/
  source/           kod C# wygenerowany przez ILSpy (projekt .csproj)
  assets/
    raw/            surowy AssetBundle
    textures/       odzyskane Texture2D jako PNG
    materials/      właściwości materiałów zapisane w JSON
  MANIFEST.json     lista zasobów i diagnostyka
scripts/
  recover_assets.py
.github/workflows/
  recover.yml
```

Automatyczny proces pobiera publicznie dostępną wersję 1.0.7 z Thunderstore i porównuje jej SHA-256 z załączoną biblioteką. Jeżeli sumy kontrolne są różne, jest to osobny build i należy zachować rozróżnienie.
