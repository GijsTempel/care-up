
Данный пакет 'Serial number license' предназначен защиты продуктов путем привязки продукта к hardwareID с использльзованием серийных номеров (DSA подпись)

Пакет влючается в себя
- Пример встраивания проверки серийных номеров (лицензий) в код приложения (игры)
- Интерфейс генерации серийных номеров (лицензий)

Функциональные возможности:
- Генерация серийных номеров на основе алгоритма DSA
- Генерация серийных номеров на основе простого алгоритма ассимитричной подписи
- Привязка серийного номера к аппартному идентификаору
- Поддержка времени истечения серийного номера



=============================================================
- Пример встраивания проверки серийных номеров (лицензий) в код приложения (игры)

\Assets\DevXUnity\Test\Scripts\SceneScript.cs
класс SceneScript является примером встраивания проверки серийного номера в код игры


\Assets\DevXUnity\SerialNumberLicense\SerialNumberVerify.cs
\Assets\DevXUnity\SerialNumberLicense\SerialNumberVerifyDSA.cs
\Assets\DevXUnity\SerialNumberLicense\SerialNumberVerifySimple.cs
классы SerialNumberVerify, SerialNumberVerifyDSA, SerialNumberVerifySimple  является вспомогательным классом проверки валидности серийных номеров

основные методы
		internal bool VerifySignature(string value, string signature)
        internal bool VerifySignature(byte[] value, string signature)
	  

\Assets\DevXUnity\SerialNumberLicense\SerialNumberValidateTools.cs
класс SerialNumberValidateTools реализует проверку серийных номеров (лицензий) на основе hardware_id и текущей даты

основные методы
        internal static bool Verify() - Verify current user serial number
        internal static string SerialNumberKey - Set or get - User Serial Number key
        static internal string HardwareID - Device Hardware ID
=============================================================



=============================================================
- Интерфейс генерации серийных номеров (лицензий)

\Assets\DevXUnity\SerialNumberLicense\Editor\SerialNumberSigner.cs
\Assets\DevXUnity\SerialNumberLicense\Editor\SerialNumberSignerDSA.cs
\Assets\DevXUnity\SerialNumberLicense\Editor\SerialNumberSignerSimple.cs

классы SerialNumberSigner,SerialNumberSignerDSA,SerialNumberSignerSimple реализует функции генерации серийных номеров

основные методы
        internal void GenerateKeys() - Generate open and close keys
        internal string Sign(string value) - Make serial number

	
\Assets\DevXUnity\SerialNumberLicense\Editor\SerialNumberGeneratorTools.cs
класс SerialNumberGeneratorTools реализует высокоуровневые функции работы с лицензиями

основные методы
        internal static void UpdateKeys(bool re_create=false, bool as_dsa=false) - Update/Create Keys
        internal static string MakeLicense(string hardware_id, DateTime? expiration_date, string comment, string email) - MakeLicense and save into folder
        internal static List<LicenseInfo> GetLicenseList() - Return all generated licenses
        internal static string CloseKey - Get or set Close key
        internal static string OpenKey - Get or set Open key


\Assets\DevXUnity\SerialNumberLicense\Editor\SerialNumberGeneratorUI.cs

класс SerialNumberGeneratorUI реализует пример пользовательского интерфейса для работы с лицензиями: "Window/DevXUnityTools-SerialNumbers"

основные методы
        internal static void LicenseGeneratorShow() - Show License Generator Tab page
        void OnGUI() -  Main GUI
=============================================================
        