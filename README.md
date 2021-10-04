# Multithreading-QuickSort

## Aufgabenstellung

Eine Quicksort-Algorithmus Implementierung, die durch die Thread Programmierung optimiert wird.  
Als Programmiersprache wird c# verwendet.  

## Was ist [Multithreading](https://de.wikipedia.org/wiki/Multithreading) ?

> Multithreading (auch Nebenläufigkeit, Mehrsträngigkeit oder Mehrfädigkeit genannt) bezeichnet in der Informatik das gleichzeitige Abarbeiten mehrerer Threads (Ausführungsstränge) innerhalb eines einzelnen Prozesses oder eines Tasks (ein Anwendungsprogramm).  

[Multithreading]: https://github.com/Lion1Blue/Multithreading-Sortieralgorithmen/blob/main/Bilder/Multithreading.png  "Multiprocessing / Multithreading"
![Multithreading]

## [Quicksort](https://de.wikipedia.org/wiki/Quicksort)-Algortihmus

Der Quick-Sort ist ein schneller, rekursiver Algorithmus der nach dem Teile und Herrsche Prinzip arbeitet.  

**[Vorgehensweise des Quicksort-Algorithmus:](https://makolyte.com/multithreaded-quicksort-in-csharp/)**

Einzelschritt:  
>Zunächst wird die zu sortierende Liste in zwei Teillisten („linke“ und „rechte“ Teilliste) getrennt. Dazu wählt Quicksort ein sogenanntes Pivotelement aus der Liste aus. Alle Elemente, die kleiner als das Pivotelement sind, kommen in die linke Teilliste, und alle, die größer sind, in die rechte Teilliste. Die Elemente, die gleich dem Pivotelement sind, können sich beliebig auf die Teillisten verteilen. Nach der Aufteilung sind die Elemente der linken Liste kleiner oder gleich den Elementen der rechten Liste.

<br>

Rekursion:  
>Anschließend muss man also noch jede Teilliste in sich sortieren, um die Sortierung zu vollenden. Dazu wird der Quicksort-Algorithmus jeweils auf der linken und auf der rechten Teilliste ausgeführt. Jede Teilliste wird dann wieder in zwei Teillisten aufgeteilt und auf diese jeweils wieder der Quicksort-Algorithmus angewandt, und so weiter. Diese Selbstaufrufe werden als Rekursion bezeichnet. Wenn eine Teilliste der Länge eins oder null auftritt, so ist diese bereits sortiert und es erfolgt der Abbruch der Rekursion.

<br>

## Warum profitiert der QuickSort von einer Parallelisierung ?

Mehrere Threads können einen Prozess beschleunigen wenn:

1. die Arbeit in mehrere, nicht überlappende Teilbreiche aufgeteilt werden kann
2. der Prozessor mehrere Kerne hat und somit mehrere Threads gleichzeitig arbeiten können

Da der Quicksort-Algorithmus das Array bei jedem Schritt in zwei sich nicht überschneidende Subarrays unterteilt, erfüllt er die erste Bedingung und die Arbeit kann parallelisiert werden.

<br>

## [Threads in c#](http://www.codeplanet.eu/tutorials/csharp/64-multithreading-in-csharp.html)

Threads sind in c# im Namespace enthalten.
````c#
using System.Threading
````

**************************************************************************************************************************************************************************

### Funktionsaufruf ohne Übergabeparameter

Um Funktionen in einem Neuen Thread zu starten, muss dem Thread im Knostruktor die auszuführende Funktion übergeben werden.

````c#
Thread newThread = new Thread(SomeFunction)
````

SomeFunction:
````c#
public static void SomeFunction()
{
    for (int i = 0; i < 100; i++)
    {
        Console.WriteLine("Counter in different Thread: " + i);
    }
}
````

Der Thread muss anschließend gestarted werden mit 
````c#
newThread.Start();
````
Auf diese Art und Weise können jedoch nur Funktionen mit keinem Übergabeparameter aufgerufen werden.  

**************************************************************************************************************************************************************************

### Funktionsaufruf mit Übergabeparameter

Will man eine Funktion mit mehreren Übergabeparametern aufrufen, ist man gezwugen die Thread-Methode Bestandteil einer Klasse zu machen, deren Eigenschaften als Argumente von dem Thread ausgelesen werden können.  
Die Übergabe erfolgt dann über die Setter der Klasse.
````c#
public MyThreadClass
{
    public MyThreadClass()
    {
    
    }
    
    public void DoWork()
    {
        for (int i = 0; i < Iterations; i += Increment)
        {
            Console.WriteLine("Counter in different Thread: " + i);
        }
    }
    
    public int Iterations { get; set; }
    public int Increment { get; set; }
}

public static void Main(string[] args)
{
    MyThreadClass threadClass = new MyThreadClass();
    
    // Übergabe der Parameter
    threadClass.Iterations = 200;
    threadClass.Increment = 3;
    
    Thread newThread = new Thread(threadClass.DoWork);
    newThread.Start();
}

````

<br>

## UML-Diagramm

[UML]: https://github.com/Lion1Blue/Multithreading-Sortieralgorithmen/blob/main/Bilder/UML-Diagramm.png "UML-Diagramm"
![UML]

#### QuickSort
- wird wie eine Bibliothek verwendet und beinhaltet die Methode Sort(double[] array, int left, int right) zum Sortieren des Arrays   
- die Methode Partition(double[], int left, int right) liefert den index des Pivotelement.  
#### AysncQuickSortWrapper
- wird verwendet, um eine Methode mit Übergabeparameter in einem anderen Thread aufzurufen, sie besitzt ein Setter für die privaten Variablen int left, right und double[] array.  
#### AsyncQuickSort
- die Methode Sort(double[] array, int left, int right) erstellt mehrere Instanzen der Klasse AsyncQuickSortWrapper, setzt die Parameter und führt die Methode Sort() (AsyncQuickSortWrapper) in unterschiedlichen Threads aus.  



