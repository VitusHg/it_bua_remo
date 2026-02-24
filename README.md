# RDP Multi-Session Manager (Windows)

Diese Anwendung ist ein **Windows-Desktop-Tool für Administratoren**, um mehrere Microsoft-RDP-Sitzungen gleichzeitig in einem Fenster mit Tabs zu verwalten.

## Für Enduser: So nutzt du die App

1. App starten.
2. Im Feld **„Hostname oder IP“** den Zielhost eintragen (z. B. `server01.firma.local` oder `10.0.0.20`).
3. Auf **„Neue RDP-Session“** klicken.
4. Es öffnet sich ein neuer Tab mit der eingebetteten RDP-Sitzung.
5. Windows/RDP zeigt den normalen Credential-Dialog (kein Passwort in der App gespeichert).
6. Für weitere Hosts einfach neue Tabs erstellen.
7. Tabs können über das kleine **„×“** im Tab geschlossen werden.
8. Bei Verbindungsabbruch wird der Tab als **„(disconnected)“** markiert.

## Welches Environment wird benötigt?

### Betriebssystem
- **Windows 10/11** oder **Windows Server** mit Desktop-Erfahrung.
- Die App ist **Windows-only** (wegen WinForms + Microsoft RDP ActiveX).

### Laufzeit für Enduser (nur Ausführen)
- Wenn als self-contained veröffentlicht: keine separate .NET-Installation nötig.
- Wenn framework-dependent veröffentlicht: **.NET 8 Desktop Runtime** erforderlich.

### Für Entwicklung/Build
- **.NET 8 SDK**
- **Visual Studio 2022** (empfohlen) mit Workload:
  - „.NET-Desktopentwicklung“
- Zugriff auf Microsoft RDP ActiveX/Terminal Services Komponenten (unter Windows vorhanden).

### Netzwerk & Berechtigungen
- Netzwerkzugriff auf Zielhost und RDP-Port (standardmäßig TCP **3389**).
- Auf Zielsystem muss RDP aktiviert sein.
- Nutzerkonto benötigt RDP-Anmelderechte.

## Starten (Entwicklung)

```bash
dotnet restore
dotnet build
dotnet run
```

> Hinweis: Build/Run funktioniert nur auf Windows mit installiertem .NET SDK.

## Veröffentlichung (Beispiel)

### Framework-dependent
```bash
dotnet publish -c Release -r win-x64 --self-contained false
```

### Self-contained
```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

Die erzeugte EXE liegt danach im `bin/.../publish`-Ordner.
