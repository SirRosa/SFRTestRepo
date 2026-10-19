# Sirroh Fur Randomizer

Wydajny system wariantów umaszczenia 9 gatunków Valheim.

## Optymalizacja
- 47 odmian skórek; 59 współdzielonych materiałów w jednym AssetBundle
- Brak tworzenia kopii materiałów na każdego moba
- Brak skanowania wszystkich shaderów sceny
- Brak globalnego patchowania LevelEffects (zgodność gwiazdek)
- Utrzymana numeracja ZDO: furRandomizerSet i furRandomizerSetActive
- Autor ZDO wybiera wariant; pozostali klienci jedynie odczytują
- Serwer headless nie ładuje materiałów
- Jednorazowa inicjalizacja i brak Update() na mobach

## Pliki
- src/Plugin.cs — całkowicie nowy kod pluginu
- src/SirrohFurRandomizer.csproj — projekt do kompilacji
- assets/SirrohFurRandomizer.furrandomizer — zasoby Unity osadzane w DLL
- assets/textures/ — edytowalne wyeksportowane PNG
- assets/materials/ — kopie parametrów materiałów w JSON
- assets/MATERIAL_CATALOG.md — powiązania skór i tekstur
- .github/workflows/build.yml — kompilacja na bibliotekach Valheim Dedicated Server

## Test
[Wersje testowe](https://github.com/SirRosa/Sirroh-Fur-Randomizer/releases)

Kompilacja to nie dowód działania w grze ani pomiar FPS. Testuj na kopii świata i bez starego moda do losowania skórek.

Odzyskane tekstury mają niezależne prawa autorskie. Przed publiczną redystrybucją sprawdź licencję bądź uzyskaj zgodę autora pierwotnych grafik. Historia odzyskiwania dostępna w historii Git.
