Publish App

```bash
dotnet publish ./WinEjectDisk/Src/App/ \
  -c Release \
  -r win-x64 \
  --self-contained false \
  -p:PublishSingleFile=true \
  -o ./dist
```

ENHANCEMENT: Implement a CI/CD like pipeline - Basically we should do the main build under `./dist/temp` the copy the files to their real version/name, move the real ones to dist root, and remove dist/temp. We could create a script to do that and also execute the dotnet publish command.

FIXME: do i need the app.manifest admin request?
FIXME: Copy the .iss file here
FIXME: how update will work
