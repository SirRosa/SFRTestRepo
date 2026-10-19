# Audyt odzyskanego Balrond Fur Randomizer 1.0.7

Dokument dotyczy **odczytanego kodu C#**, a nie potwierdzonych błędów na Valheim 1.0. Nie wykonano testów gry.

## Najważniejsze obszary do przeglądu

1. **Harmony patch wszystkich mobów** (Launch.cs, LevelEffects_SetupLevelVisualization_Patch.Prefix): Prefix zwraca false. Kod zastępuje standardową metodę LevelEffects.SetupLevelVisualization również dla stworzeń bez niestandardowych skór; może kolidować z innymi modami zmieniającymi poziomy/gwiazdki, a także zmianami API gry.
2. **Ragdolle i współdzielony prefab** (Launch.cs, Character_OnDeath_Patch.Prefix): przed śmiercią m_mainModel.sharedMaterials jest ustawiane na prefabie efektu śmierci, niekoniecznie na osobnej instancji. Wymaga testu wielokrotnych śmierci różnych wariantów.
3. **Alokacje materiałów** (FurRandomizer.cs, SetSkin i Launch.cs): wiele wywołań new Material(...) przy zmianie wyglądu. Warto sprawdzić wycieki instancji i strategię zwalniania pamięci.
4. **Sieć** (FurRandomizer.cs, PickRandomSet): wybór wariantu zapisywany jest do ZDO jako furRandomizerSet / furRandomizerSetActive. Sprawdź autorytet zapisu (własność ZDO), synchronizację i kompatybilność z istniejącymi światami.
5. **Skórki potomstwa** (FurRandomizer.cs i ścieżki spawn): oryginalny mod nie dziedziczy wariantów po rodzicach. Warto rozstrzygnąć, czy implementować dziedziczenie, czy zachować losowanie.
6. **Unity shader/material** (ModResourceLoader.cs, ShaderReplacment.cs): właściwości shaderów i materiały są z poprzedniego AssetBundle; trzeba sprawdzić zgodność z rendererem obecnego Valheim.
7. **Zawartość danych** (recovered/assets/MATERIAL_CATALOG.md): po nazwach prefabu i wariantu łatwo sprawdzić powiązania _MainTex, _BumpMap i _EmissionMap oraz wskazać dokładny PNG do poprawy.

## Ważne ustalenia

- Wejściowa DLL odpowiada **bit po bicie** publicznej wersji Thunderstore 1.0.7.
- 69 obiektów Texture2D jest eksportowanych jako PNG. Nie oznacza to 69 różnych stworzeń; są tam także mapy normalne, emisji oraz różne warianty.
- 59 obiektów Material ma wyeksportowane serializowane właściwości do JSON; zachowano identyfikatory PathID i ścieżki AssetBundle, dlatego identyczne nazwy materiałów nie są mieszane.
- 3 obiekty Shader mają zrzuty metadata; to nie oznacza odzyskania oryginalnego kodu shaderów w postaci shader source.
- W odzyskanym projekcie ILSpy dostępny jest oryginalny osadzony plik BalrondFurRandomizer.furrandomizer.
- Osobny projekt src/SirrohFurRandomizer.csproj pozwala pracować na odzyskanym kodzie bez modyfikowania wygenerowanego drzewa, jednak kompilacja wymaga lokalnych bibliotek gry.

## Kolejność dalszych prac

1. Uzyskać kompilację dla zależności Valheim 1.0 + BepInEx 5.
2. Usunąć globalne nadpisanie LevelEffects, jeżeli powoduje konflikty z systemem poziomów.
3. Naprawić przypisywanie materiałów ragdolli i potencjalne alokacje bez zwalniania.
4. Poprawić tekstury w PNG, odbudować AssetBundle w zgodnej wersji Unity.
5. Testować multiplayer oraz stałość kluczy ZDO po przejściu na nową DLL.

**Nie uruchamiać jednocześnie obu modów bez przeglądu konfliktów Harmony i GUID.**
