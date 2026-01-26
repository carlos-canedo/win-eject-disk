Publish App

```bash
dotnet publish ./WinEjectDisk/Src/App/ \
  -c Release \
  -r win-x64 \
  --self-contained false \
  -p:PublishSingleFile=true \
  -o ./dist
```

Kill running app process

```
Get-Process -Name "WinEjectDisk*" | Stop-Process -Force
```

- ENHANCEMENT: Add _github actions_ to replcate release-build in prod
