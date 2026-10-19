# Kompilacja Sirroh Fur Randomizer

Repozytorium zawiera dekompilację oryginalnej DLL, odzyskane tekstury PNG, właściwości materiałów i surowy AssetBundle. Edycja PNG nie aktualizuje automatycznie osadzonego w DLL AssetBundle.

## Zawartość

- Kod: recovered/source/BalrondFurRandomizer/
- Bundle: recovered/assets/raw/BalrondFurRandomizer.furrandomizer
- PNG: recovered/assets/textures/
- Materiały JSON: recovered/assets/materials/
- Mapa powiązań: recovered/assets/MATERIAL_CATALOG.md i MATERIAL_TEXTURE_MAP.json
- Kompilacja: src/SirrohFurRandomizer.csproj

## Pierwsza kompilacja

Zainstaluj .NET 10 SDK. Użyj bibliotek własnej instalacji Valheim i BepInEx 5.

~~~powershell
dotnet build src/SirrohFurRandomizer.csproj -c Release -p:ValheimManagedDir="C:\Steam\steamapps\common\Valheim\valheim_Data\Managed" -p:BepInExCoreDir="C:\Steam\steamapps\common\Valheim\BepInEx\core"
~~~

Możesz także umieścić referencje w katalogu _references/Managed i _references/BepInEx/core. Te foldery są ignorowane przez Git, aby biblioteki gry nie trafiły do publicznego repozytorium.

**Status:** kompilacja pod Valheim 1.0 pozostaje NIEZWERYFIKOWANA. Nie mamy tutaj kompletu bibliotek gry do uruchomienia builda. ILSpy może wyprodukować kod wymagający ręcznego poprawienia po dekompilacji.

## Zmiana tekstur

1. Znajdź materiał danego stworzenia w MATERIAL_CATALOG.md.
2. Ustal powiązany PNG przez slot _MainTex i mapę PathID.
3. Edytuj PNG z zachowaniem UV i kanału przezroczystości.
4. W zgodnym Unity zaimportuj PNG, utwórz materiały z parametrami JSON i przebuduj AssetBundle. Zamień plik recovered/assets/raw/BalrondFurRandomizer.furrandomizer.
5. Skompiluj projekt. Plik AssetBundle zostanie osadzony w DLL jako BalrondFurRandomizer.furrandomizer.

Odzyskane JSON nie są oryginalnymi plikami .mat. Pełny oryginalny projekt Unity, jego meta pliki i shader source nie są automatycznie odzyskane.

## Zgodność i tożsamość

Odzyskany kod nadal zawiera stary identyfikator BepInEx oraz klucze ZDO furRandomizerSet i furRandomizerSetActive. Przed jednoczesnym używaniem forka i oryginału należy rozwiązać konflikt patchy i identyfikatorów; zmiana kluczy ZDO bez migracji może utracić przypisania skórek.

## Prawa

Nie zakładaj, że dekompilacja przenosi licencję lub prawo do redystrybucji zasobów oryginalnego autora. Uzyskaj zgodę na publiczne wydanie forka.
