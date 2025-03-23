
指定されたテキストファイルが追記されたら追記された部分を A.I.VOICE Editor で読み上げます。
使い道として、例えば Vcot (https://booth.pm/ja/items/5086553) の History.txt を監視対象にするとキャラクターの発言を A.I.VOICE のキャラに読み上げてもらえます。

## 使い方

.exe と同じディレクトリに以下のような config.json を配置して実行します。
```JSON
{
  "WatchFilePath": "path/to/Vcot/History.txt",
  "Mark": "<avatar-name>：",
  "Voice": "<voice-name>"
}
```

`<avatar-name>` には Vcot で読み込むアバターの名前を指定します。
`<voice-name>` には A.I.VOICE Editor で読み上げてもらうキャラの名前を指定します。（現在は未実装。この値によらずデフォルトで指定されるボイスで読み上げます）


