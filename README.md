# CO2CounterStatus
このBeatSaberプラグインは、I/O DATA製の[UD-CO2S](https://www.iodata.jp/product/tsushin/iot/ud-co2s/index.htm)を使って、二酸化炭素濃度と温度と湿度を表示する[CountersPlus](https://github.com/Caeden117/CountersPlus)用のカスタムカウンターです。
また、[HttpSiraStatus](https://github.com/denpadokei/HttpSiraStatus)を使用して[オーバーレイ](https://github.com/rynan4818/beat-saber-overlay)に送信して表示することもできます。
他にも、[PlayerInfoViewer](https://github.com/rynan4818/PlayerInfoViewer)でメニュー画面に表示することも可能です。

![image](https://user-images.githubusercontent.com/14249877/219855750-6605731c-b134-46a0-9594-ea347817b993.png)

![image](https://user-images.githubusercontent.com/14249877/219855758-d576af60-0283-491a-8770-96e413239d37.png)

# インストール方法
1. CountersPlusを使用する場合には[CountersPlus](https://github.com/Caeden117/CountersPlus)をインストールして動作するようにしてください。
2. オーバーレイを使用する場合には[HttpSiraStatus](https://github.com/denpadokei/HttpSiraStatus)と[Beat Saber Overlay 改良版](https://github.com/rynan4818/beat-saber-overlay)をインストールして動作するようにしてください。本modの表示には`index_HDT_SRMqueue_CO2_sample.html`を使用する必要があります。
3. [リリースページ](https://github.com/rynan4818/CO2CounterStatus/releases)から最新のmodのリリースをダウンロードします。
4. ダウンロードしたzipファイルをBeat Saberフォルダに解凍して、`Plugin`フォルダにコピーします。modは3種類ありますので必要なものをダウンロードしてください。
    - CO2Core : **本Modの本体で必ず必要です**
    - CO2Counter : CountersPlus用のカスタムカウンターです
    - HttpCO2Status : HttpSiraStatusを使ってオーバーレイにデータを送信します

# 使用方法
* Mod設定のCO2Core設定で、UD-CO2S のCOMポートを設定してください。COMポートはUD-CO2Sの[Windows用アプリ](https://www.iodata.jp/lib/software/c/2284.htm)の設定の詳細設定で確認してください。

![image](https://user-images.githubusercontent.com/14249877/219856680-ccced33d-3a0c-4acc-8258-9cd64638a8a5.png)

* CO2Counterの場合はCOUNTERS+の設定画面にHDT Counterが追加されますので、表示位置や詳細設定をして使用してください。

![image](https://user-images.githubusercontent.com/14249877/219857331-6437bfda-ff56-446e-9643-ac955916af0a.png)
![image](https://user-images.githubusercontent.com/14249877/219857368-313a5d9f-c95c-4638-be67-757d77a06caf.png)
![image](https://user-images.githubusercontent.com/14249877/219857566-2a1216b5-b2ba-42c5-9c54-1fee9d0f61c0.png)

CO2Counterの設定値は以下の通りです。
|項目|説明|
|:---|:---|
|DecimalPrecision|HDTの小数点以下を表示する桁数|
|EnableLabel|ラベル(Head Distance Travelled)の表示|
|LabelFontSize|ラベルのフォントサイズ|
|FigureFontSize|カウンターのフォントサイズ|
|OffsetX|カウンターのX軸オフセット|
|OffsetY|カウンターのY軸オフセット|
|OffsetZ|カウンターのZ軸オフセット|

数値を細かく設定したい場合は、`UserData\HDTCounter.json`を直接編集してください。

以下の設定はUIないため、直接設定ファイルを変更してください。

|項目|説明|
|:---|:---|
|LabelText|ラベルの表示文字|
