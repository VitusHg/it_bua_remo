# RDP Multi-Session Manager (Windows)

> Kurz gesagt: Mit dieser App kannst du mehrere Remote-Desktop-Verbindungen (RDP) **gleichzeitig in einem einzigen Fenster mit Tabs** öffnen.

---

## 1) Ich bin absoluter Laie – was muss ich können?

Du musst **kein C#** können und nichts programmieren.

Wenn du die App nur benutzen willst, brauchst du nur:
- einen Windows-PC,
- die fertige `*.exe` der App,
- Zugangsdaten für die Server, auf die du dich per RDP verbinden darfst.

---

## 2) Was brauche ich als Endnutzer wirklich?

## Pflicht
- **Windows 10 oder 11** (oder Windows Server mit Desktop).
- Netzwerkzugriff auf den Zielserver (RDP, meistens Port **3389**).
- Ein Benutzerkonto mit RDP-Berechtigung auf dem Zielserver.

## Eventuell nötig
- Falls die App **nicht** als „Self-contained“ geliefert wurde:
  - installiere die **.NET 8 Desktop Runtime**.

## Nicht nötig
- Kein Visual Studio.
- Kein .NET SDK.
- Keine Programmierkenntnisse.

---

## 3) Schritt-für-Schritt: App benutzen

1. `RdpMultiSessionManager.exe` starten.
2. In das Feld **„Hostname oder IP“** den Server eintragen (z. B. `10.0.0.20`).
3. Auf **„Neue RDP-Session“** klicken.
4. Ein neuer Tab wird geöffnet.
5. Im Windows-Anmeldefenster Benutzername/Passwort eingeben.
6. Für weitere Server einfach wieder Schritt 2–5.
7. Tabs kannst du über das **„×“** schließen.
8. Wenn eine Verbindung weg ist, steht im Tab **„(disconnected)“**.

---

## 4) Häufige Probleme (einfach erklärt)

### „Ich kann nicht verbinden“
Prüfe:
- Ist der Servername / die IP korrekt?
- Ist der Server eingeschaltet und im Netzwerk erreichbar?
- Ist RDP am Zielserver aktiviert?
- Darf dein Benutzerkonto sich per RDP anmelden?
- Ist die Firewall für RDP (Port 3389) offen?

### „Es passiert gar nichts beim Start“
- Wahrscheinlich fehlt die Laufzeit.
- Installiere die **.NET 8 Desktop Runtime** (nur wenn die App nicht self-contained verteilt wurde).

---

## 5) Für Entwickler (nur wenn du selbst bauen willst)

Dieser Teil ist **nur** für Personen, die den Quellcode kompilieren möchten.

Benötigt:
- Windows 10/11
- **.NET 8 SDK**
- optional Visual Studio 2022 mit „.NET-Desktopentwicklung“

Befehle:

```bash
dotnet restore
dotnet build
dotnet run
```

Veröffentlichen (EXE erzeugen):

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

Danach liegt die EXE im `bin/.../publish`-Ordner.
