# Przegląd Bazy
## Wstęp
Niniejszy program jest prostą aplikacją tworzoną na zaliczenie z Zaawansowane Progrmanowania Obiektowego. 
Już z założenia miał być to program prosty, który miał służyć prowadzącemu do porównania wydajności wykonania tego samego zadania przez program oparty na .NET a programem opartym na JVM. 

Problemem w tym zadaniu była taka rezliacja pobierania i przetwarzania danych, aby jak najmniej było ich na komputerze klienta.
Jest to spowodowane tym, że baza danych zwiera gigabajty punktów pomiarowych i przetwarzanie tego lokalnie jest strasznie mozolne.

Generalnie założone zadanie udało się wykonać - prosta aplikacja (która jest prezentowana w tym repozytorium) pobiła o głowę tę stosowaną przez prowadzącego. 
## Wykorzystane technologie
* LINQ to SQL (Entity Freamwork 6),
* XAML, 
* OxyPlot,
* oraz wiele pomniejszych elementów środowiska .NET.

## Podsumowanie
Udało się zrealizować aplikację, która spełnia wymagania prowadzącego. 
Zawiera ona jednak parę niedoskonałości, mianowicie:
* nie odizolowałem warstwy We/Wy (GUI) od logiki aplikacji,
* nie wszystkie stałe zostały wyciągnięte do odpowiadających im właściwości klas.