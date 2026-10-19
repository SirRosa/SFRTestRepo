# Sirroh Fur Randomizer

Projekt kontynuacji Balrond Fur Randomizer, oparty na **kompletnym odzysku danych dostępnych w bibliotece BalrondFurRandomizer.dll**.

## Potwierdzony stan

- DLL użytkownika to dokładnie Balrond furRandomizer 1.0.7 (identyczny SHA-256).
- 12 plików C# po dekompilacji ILSpy (oryginalnych źródeł autora nie da się odzyskać 1:1).
- 69 tekstur Texture2D w PNG.
- 59 materiałów + 3 shadery z metadanymi Unity.
- Surowy Unity AssetBundle i projekt C# do dalszych zmian.
- 0 błędów podczas ekstrakcji.
- **Kompilacja na Valheim 1.0: jeszcze niezweryfikowana.**

## Pliki

| Ścieżka | Zawartość |
|---|---|
| recovered/source/ | Kod C# z ILSpy i oryginalny embedded AssetBundle |
| recovered/assets/textures/ | Edytowalne eksporty PNG |
| recovered/assets/materials/ | Material / Shader JSON z wartościami i referencjami |
| recovered/assets/raw/ | Oryginalny Unity AssetBundle |
| recovered/assets/MATERIAL_CATALOG.md | **Stworzenie → wariant → materiał → dokładna tekstura** |
| recovered/assets/MATERIAL_TEXTURE_MAP.json | Kompletny indeks PathID i slotów shaderów |
| recovered/assets/MANIFEST.json | Lista wszystkich obiektów i błędów |
| src/SirrohFurRandomizer.csproj | Nowy projekt C# przygotowany pod własne referencje gry |
| BUILDING.md | Jak zbudować DLL i ponownie osadzić tekstury |
| AUDIT.md | Zidentyfikowane zagrożenia kompatybilności i obszary napraw |

## Użycie

Do poprawiania tekstur zacznij od recovered/assets/MATERIAL_CATALOG.md. Do zmian mechaniki zajrzyj do recovered/source/BalrondFurRandomizer/.

GitHub Actions: workflow Recover Balrond Fur Randomizer dekompiluje dokładny publiczny build i aktualizuje pliki w recovered/. Jego uruchomienie nadpisuje zmiany wykonane bezpośrednio w recovered/. Do rozwoju produkcyjnego korzystaj z niezależnych plików/projektu w src/ lub wyłącz automatyczną regenerację po zakończeniu odzysku.

## Ograniczenia i prawa

- Nie odzyskano oryginalnego projektu Unity, historii Git ani komentarzy autora.
- Materiały JSON nie są gotowymi plikami .mat; poprawione PNG należy ponownie spakować do AssetBundle.
- Nie dodawaj do repo bibliotek Valheim/BepInEx z lokalnej instalacji: _references/ jest ignorowany przez Git.
- Przed redystrybucją odzyskanych grafik i kodu potrzebne jest sprawdzenie praw autorskich oryginalnego autora.
