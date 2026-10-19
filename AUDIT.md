# Audyt i ryzyka Sirroh Fur Randomizer

## Wdrożone zmiany
- Usunięto globalny patch LevelEffects, intensywne skanowanie shaderów i duplikowanie materiałów przy każdym spawnie.
- Wczytanie materiałów i ich katalogowanie odbywa się tylko raz.
- Komponent pomocniczy zostaje usunięty po przypisaniu skóry.
- Klucze ZDO zachowane w celu migracji wcześniejszych zapisów.
- Serwer bez grafiki ogranicza się do ustawienia indeksów wariantów.

## Nadal wymaga testów
- Shadery na różnych konfiguracjach GPU w Valheim 1.0.
- Wpływ na FPS/CPU i współpraca z dużą liczbą mobów.
- Multiplayer: nowy gracz dołączający, zmiana właściciela ZDO, oswojone zwierzęta.
- Lox i Hatchling: nazwy rendererów i slotów.
- Ragdolle pozostają vanilla, aby nie modyfikować współdzielonych efektów śmierci.

Nie deklarujemy jeszcze, że mod jest stabilny w grze ani że wydajność poprawiła się o określoną liczbę procent. Wymagane testy z profilerem.
