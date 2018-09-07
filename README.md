# AutoMasshTik

Mass MikroTik software and firmware update tool

## About

AutoMasshTik is a cross platform tool that allows network admins that run networks on MikroTiks to execute mass upgrade of software on their devices. By using this software you can simply insert (or copy paste) a list of IP addresses of your MikroTik devices insert credentials and port number (if default value of 22 is changed to something else) and choose between three options:

- Test connection - SSH conection is established (on the right side of the window you can see the result of the test).
- Update Packages - SSH command is sent to MikroTik: /system package update install
- Update Firmware And Reboot - SSH commands are sent to MikroTik: /system routerboard upgrade and after couple of seconds /system reboot (y)

## Installation

Installation is simple - after downloading and running setup.exe software will be installed (in fact it is quite portable - it runs independently of .NET framework installed on the machine). Shortcut will be placed in Start menu and Desktop. Software automatically checks for updates at start.

Tech

Based on these technologies

- [Avalonia UI](https://github.com/AvaloniaUI/Avalonia)
- [Righthand.SharpRedux](https://github.com/MihaMarkic/sharp-redux)
- [SSH.NET](https://github.com/sshnet/SSH.NET/)
- Fody
- Newtonsoft.Json
- NLog
- Righthand.Immutable

And of course, AutoMasshTik itself is open source with a public repository on [GitHub](https://github.com/MihaMarkic/AutoMasshTik).

License

MIT

Free Software, Hell Yeah!

Made with ❤️ by:
[![N|Righthand](http://blog.rthand.com//images/Logo_vNext.png)](http://blog.rthand.com) and [![N|Wlan](http://wlan.novagorica.eu/uploads/4/4/1/0/4410231/1183677.png)](http://wlan.novagorica.eu)