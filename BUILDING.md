# Kompilacja Sirroh Fur Randomizer

Automatyczny workflow .github/workflows/build.yml pobiera Valheim Dedicated Server 1.0 (SteamCMD app 896660, gałąź public), obecny BepInExPack Valheim, kompiluje src/SirrohFurRandomizer.csproj i tworzy prerelease ZIP w formacie Thunderstore.

Lokalnie zainstaluj .NET SDK 8+ i wskaż katalogi bibliotek gry oraz BepInEx (aktualna wersja):

dotnet build src/SirrohFurRandomizer.csproj -c Release -p:GameManagedDir=... -p:BepInExCoreDir=...

Zmiana samej grafiki PNG nie zmienia osadzonego AssetBundle. Po poprawie tekstury trzeba przebudować paczkę Unity i zastąpić assets/SirrohFurRandomizer.furrandomizer. PNG i mapa wariantów są zachowane w assets/.

Nie publikuj w repo bibliotek Valheim. Testy gry na backupie świata; usuń starą wersję moda przed testem.
