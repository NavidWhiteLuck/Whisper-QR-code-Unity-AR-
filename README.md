# Whisper QR code Unity AR
این پروژه با هدف کمک هرچه بیشتر به جامعه مدنی برای تبادل نظرات توسط من ایجاد شد. در این پروژه سعی شده تا حد ممکن قابلیتی ایجاد شود که در آن کاربران بتوانند در نقش یک فرد در جهان فیزیکی در مکانی حضور پیدا کنند و با یک کد (کیو آر) ساده به تبادل نظرات درباره با آن مکان بپردازند و آن نظرات را در همان محل به شتراک بگذارند. یا مدیریت و مجری هر محل با ایجا این کد ها به دریافت نظرات حاضران بپردازد.
# اپلیکیشن اسکن و اشتراک‌گذاری QR Code با AR و کامنت‌گذاری

## مقدمه
این پروژه یک اپلیکیشن اندرویدی است که به کاربران امکان می‌دهد QR Code تولید کنند، آن را اسکن نمایند و روی آن کامنت بگذارند. کاربران می‌توانند از واقعیت افزوده (AR) برای مشاهده اطلاعات QR Code استفاده کنند و نظرات ثبت‌شده را ببینند.

## اهداف پروژه
- ایجاد و اسکن QR Code توسط کاربران
- امکان اضافه کردن و مشاهده کامنت‌ها روی هر QR Code
- ذخیره‌سازی اطلاعات در PlayFab برای دسترسی کاربران دیگر
- پشتیبانی از ورود با Google
- قابلیت دانلود و اشتراک‌گذاری QR Code
- تنظیم دسترسی برای نمایش یا عدم نمایش کامنت‌ها

## تکنولوژی‌های استفاده‌شده
- **Unity** (موتور بازی‌سازی و توسعه اپلیکیشن)
- **AR Foundation** (برای واقعیت افزوده)
- **AR Core** (برای واقعیت افزوده)
- **ZXing** (برای اسکن QR Code)
- **PlayFab** (برای ذخیره‌سازی اطلاعات در فضای ابری)
- **Firebase Authentication** (برای ورود با Google)
- **DOTween** (برای انیمیشن‌های نرم در UI)
- **TextMesh Pro** (برای نمایش بهتر متون در UI)

## نحوه‌ی پیاده‌سازی

### ۱. ورود کاربران
کاربران از طریق Google Sign-In وارد می‌شوند و نام آن‌ها در PlayFab ذخیره می‌شود. نام کاربری قابل ویرایش است.

### ۲. ایجاد QR Code
- کاربر می‌تواند یک QR Code جدید تولید کند.
- QR Code شامل یک شناسه یکتا (UUID) و یک لینک سفارشی مانند `https://p7z.ir/qrcode/{id}` خواهد بود.
- امکان دانلود تصویر QR Code و اشتراک‌گذاری آن در اپلیکیشن‌های مختلف وجود دارد.

### ۳. اسکن QR Code و نمایش اطلاعات
- اسکن QR Code از طریق ZXing انجام می‌شود.
- اطلاعات مربوط به QR Code از PlayFab دریافت شده و نمایش داده می‌شود.
- اگر QR Code مربوط به یک کامنت خصوصی باشد، فقط صاحب QR Code می‌تواند کامنت‌ها را مشاهده کند.

### ۴. کامنت‌گذاری و مدیریت نظرات
- کاربران می‌توانند روی QR Codeهای اسکن‌شده کامنت بگذارند.
- همه کاربران می‌توانند کامنت‌های عمومی را ببینند.
- هر کاربر فقط می‌تواند کامنت‌های خودش را حذف کند.
- کامنت‌ها در PlayFab ذخیره می‌شوند.

### ۵. دانلود و اشتراک‌گذاری QR Code
- QR Code تولید شده به صورت تصویر ذخیره می‌شود.
- کاربران می‌توانند QR Code را در واتساپ، تلگرام و سایر شبکه‌های اجتماعی به اشتراک بگذارند.

## تست و بهینه‌سازی

### ۱. تست عملکرد
- بررسی ورود و ثبت‌نام کاربران
- تست ایجاد و اسکن QR Code در دستگاه‌های مختلف
- تست ارسال و دریافت کامنت‌ها
- بررسی تنظیمات سطح دسترسی کامنت‌ها

### ۲. بهینه‌سازی عملکرد
- استفاده از Local Cache برای کاهش درخواست‌های PlayFab
```C#
  private Dictionary<string, string> qrCodeCache = new Dictionary<string, string>();

public void FetchQRCodeData(string qrId)
{
    if (qrCodeCache.ContainsKey(qrId))
    {
        Debug.Log("Loaded from cache: " + qrCodeCache[qrId]);
        DisplayQRCodeData(qrCodeCache[qrId]);
        return;
    }

    PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
    {
        if (result.Data.ContainsKey("QRCode_" + qrId))
        {
            string qrData = result.Data["QRCode_" + qrId].Value;
            qrCodeCache[qrId] = qrData;
            DisplayQRCodeData(qrData);
        }
        else
        {
            Debug.Log("No data found for QR Code.");
        }
    }, error => Debug.LogError("Error fetching QR Code data: " + error.ErrorMessage));
}
```
- نمایش ۲۰ کامنت آخر به جای دریافت کل کامنت‌ها برای بهینه‌سازی سرعت
```
private const int MaxCommentsToShow = 20;

public void FetchComments(string qrId)
{
    PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
    {
        if (result.Data.ContainsKey("Comments_" + qrId))
        {
            List<string> comments = result.Data["Comments_" + qrId].Value.Split(';').ToList();
            comments = comments.Take(MaxCommentsToShow).ToList(); // فقط ۲۰ کامنت نمایش بده

            DisplayComments(comments);
        }
        else
        {
            Debug.Log("No comments found.");
        }
    }, error => Debug.LogError("Error fetching comments: " + error.ErrorMessage));
}
```
- استفاده از DOTween برای انیمیشن‌های روان در رابط کاربری

## نتیجه‌گیری
این پروژه یک اپلیکیشن کاربردی برای اسکن، ایجاد و اشتراک‌گذاری QR Code همراه با قابلیت AR و کامنت‌گذاری ارائه می‌دهد. با استفاده از این اپلیکیشن، کاربران می‌توانند محتواهای خود را در بستر QR Code مدیریت کرده و با دیگران به اشتراک بگذارند.

## منابع

- **Unity Documentation**: [https://docs.unity3d.com](https://docs.unity3d.com)
- **PlayFab Documentation**: [https://docs.microsoft.com/en-us/gaming/playfab/](https://docs.microsoft.com/en-us/gaming/playfab/)
- **ZXing Library for QR Code Scanning**: [https://github.com/zxing/zxing](https://github.com/zxing/zxing)
- **Firebase Authentication**: [https://firebase.google.com/docs/auth](https://firebase.google.com/docs/auth)
- **DOTween Animation Library**: [https://dotween.demigiant.com](https://dotween.demigiant.com)
- **AR Foundation in Unity**: [https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest)
