# رفع المشروع إلى GitHub

## 1. إنشاء المستودع

1. افتح `https://github.com/new`.
2. اكتب اسم المستودع: `it-help-desk-system`.
3. اختر `Public` ليظهر المشروع في ملفك الوظيفي.
4. لا تختر إضافة README أو `.gitignore` أو License؛ جميعها موجودة في المشروع.
5. اضغط `Create repository`.

## 2. رفع الملفات

بعد فك ضغط المشروع، افتح مجلد `HelpDeskSystem` في Visual Studio ثم اختر `View > Terminal` ونفذ:

```bash
git init
git add .
git commit -m "feat: publish IT help desk system"
git branch -M main
git remote add origin https://github.com/5mlan/it-help-desk-system.git
git push -u origin main
```

إذا طلب GitHub تسجيل الدخول، أكمله من نافذة Visual Studio أو المتصفح. لا تضع كلمة مرور GitHub داخل ملفات المشروع.

## 3. إعداد صفحة المستودع

استخدم الوصف التالي:

```text
Role-based IT support ticket system built with ASP.NET Core MVC, Identity, EF Core, and SQLite.
```

أضف Topics التالية:

```text
aspnet-core mvc entity-framework-core sqlite identity helpdesk ticketing-system arabic rtl
```

## 4. إضافة الصور

شغل المشروع والتقط صورًا لصفحة تسجيل الدخول، لوحة المعلومات، صفحة التذكرة، وإدارة المستخدمين. ضعها في مجلد `screenshots` ثم أضف قسمًا إلى README مثل:

```markdown
## Screenshots

![Dashboard](screenshots/dashboard.png)
![Ticket details](screenshots/ticket-details.png)
```

## 5. التأكد من نجاح المشروع

بعد الرفع افتح تبويب `Actions`. يجب أن يظهر Workflow باسم `.NET CI`. العلامة الخضراء تعني أن Restore وBuild نجحا.

إذا فشل، افتح التشغيل الفاشل واقرأ أول رسالة خطأ باللون الأحمر، أصلحها محليًا، ثم نفذ:

```bash
git add .
git commit -m "fix: resolve build error"
git push
```

## 6. تحديث المشروع لاحقًا

بعد كل تعديل:

```bash
git add .
git commit -m "feat: describe the new feature"
git push
```

استخدم `feat:` للميزات، و`fix:` للإصلاحات، و`docs:` للتوثيق، و`style:` لتعديلات التصميم.
