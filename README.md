# Was ist ein Singleton
Das Singleton Pattern ist ein Muster (Creational Pattern) und sorgt für folgende Eigenschaften:
- Es gibt genau nur ***eine*** Instanz einer Klasse im ganzen Programm
- Diese Instanz ist global zugreifbar

## Anwendungsbereiche
Typischerweise werden Singleton Pattern bei Logging (Ein Logger pro Anwendung), Konfiguration (zentrale Settings), Resource  Manager (Verbindungspool, Drucker-Queue), usw. verwendet.

## Vor- und Nachteile
Vorteile:
- Kontrolle: Es gibt garantiert nur eine Instanz
- Globaler Zugriff: Einfach von überall erreichbar
- Nützlich, wenn die Domäne das rechtfertigt (z.B. nur ein Datenbank-Verbindungspool)

Nachteile:
- Globale Zustände erschweren das Testen (Mocking)
- Versteckte Abhängigkeiten: Klassen greifen plötzlich überall auf Singleton zu
- Kann leicht missbraucht werden -> Plötzlich Singleton für alles (anti-pattern)
- Schwer zu erweitern oder auszutauschen (z.B. in Unit Tests)

## Bessere Alternative 
Eine bessere Methode ist hierbei die Dependency Injection (DI). Diese wird in einer anderen Repo nochmal detailierter erklärt. 

## Merkregeln
Singleton nur verwenden, wenn wirklich genau eine Instanz sinnvoll ist und man diese global benötigt. In modernen Architekturen meist durch DI ersetzt. 

### Mini-Übungen
Mini-Übungen (10–15 Min)
Stateful Singleton
Füge dem Logger einen Zähler hinzu (wie viele Nachrichten geloggt wurden) und gib ihn am Ende aus. Prüfe nebenläufiges Verhalten (mehrere Task.Run).
Konfigurations-Singleton
Implementiere AppConfig.Instance mit Dictionary<string,string> und Get/Set. Diskutiere: Wäre DI hier sinnvoller?
Austauschbarkeit testen
Baue eine Klasse DiagnosticsService mit Abhängigkeit zu ILoggerService (DI-Version). Schreibe einen kleinen „FakeLogger“ in der Main, der statt auf Console in eine Liste schreibt, und verifiziere die Einträge.