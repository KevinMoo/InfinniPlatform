﻿using System;
using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.BlobStorage
{
    /// <summary>
    /// Предоставляет метод для получения формат данных файла.
    /// </summary>
    public sealed class FileExtensionTypeProvider
    {
        public static readonly FileExtensionTypeProvider Default = new FileExtensionTypeProvider();


        public FileExtensionTypeProvider()
        {
            _mappings.Add(".323", "text/h323");
            _mappings.Add(".3g2", "video/3gpp2");
            _mappings.Add(".3gp2", "video/3gpp2");
            _mappings.Add(".3gp", "video/3gpp");
            _mappings.Add(".3gpp", "video/3gpp");
            _mappings.Add(".aac", "audio/aac");
            _mappings.Add(".aaf", "application/octet-stream");
            _mappings.Add(".aca", "application/octet-stream");
            _mappings.Add(".accdb", "application/msaccess");
            _mappings.Add(".accde", "application/msaccess");
            _mappings.Add(".accdt", "application/msaccess");
            _mappings.Add(".acx", "application/internet-property-stream");
            _mappings.Add(".adt", "audio/vnd.dlna.adts");
            _mappings.Add(".adts", "audio/vnd.dlna.adts");
            _mappings.Add(".afm", "application/octet-stream");
            _mappings.Add(".ai", "application/postscript");
            _mappings.Add(".aif", "audio/x-aiff");
            _mappings.Add(".aifc", "audio/aiff");
            _mappings.Add(".aiff", "audio/aiff");
            _mappings.Add(".application", "application/x-ms-application");
            _mappings.Add(".art", "image/x-jg");
            _mappings.Add(".asd", "application/octet-stream");
            _mappings.Add(".asf", "video/x-ms-asf");
            _mappings.Add(".asi", "application/octet-stream");
            _mappings.Add(".asm", "text/plain");
            _mappings.Add(".asr", "video/x-ms-asf");
            _mappings.Add(".asx", "video/x-ms-asf");
            _mappings.Add(".atom", "application/atom+xml");
            _mappings.Add(".au", "audio/basic");
            _mappings.Add(".avi", "video/x-msvideo");
            _mappings.Add(".axs", "application/olescript");
            _mappings.Add(".bas", "text/plain");
            _mappings.Add(".bcpio", "application/x-bcpio");
            _mappings.Add(".bin", "application/octet-stream");
            _mappings.Add(".bmp", "image/bmp");
            _mappings.Add(".c", "text/plain");
            _mappings.Add(".cab", "application/vnd.ms-cab-compressed");
            _mappings.Add(".calx", "application/vnd.ms-office.calx");
            _mappings.Add(".cat", "application/vnd.ms-pki.seccat");
            _mappings.Add(".cdf", "application/x-cdf");
            _mappings.Add(".chm", "application/octet-stream");
            _mappings.Add(".class", "application/x-java-applet");
            _mappings.Add(".clp", "application/x-msclip");
            _mappings.Add(".cmx", "image/x-cmx");
            _mappings.Add(".cnf", "text/plain");
            _mappings.Add(".cod", "image/cis-cod");
            _mappings.Add(".cpio", "application/x-cpio");
            _mappings.Add(".cpp", "text/plain");
            _mappings.Add(".crd", "application/x-mscardfile");
            _mappings.Add(".crl", "application/pkix-crl");
            _mappings.Add(".crt", "application/x-x509-ca-cert");
            _mappings.Add(".csh", "application/x-csh");
            _mappings.Add(".css", "text/css");
            _mappings.Add(".csv", "application/octet-stream");
            _mappings.Add(".cur", "application/octet-stream");
            _mappings.Add(".dcr", "application/x-director");
            _mappings.Add(".deploy", "application/octet-stream");
            _mappings.Add(".der", "application/x-x509-ca-cert");
            _mappings.Add(".dib", "image/bmp");
            _mappings.Add(".dir", "application/x-director");
            _mappings.Add(".disco", "text/xml");
            _mappings.Add(".dlm", "text/dlm");
            _mappings.Add(".doc", "application/msword");
            _mappings.Add(".docm", "application/vnd.ms-word.document.macroEnabled.12");
            _mappings.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            _mappings.Add(".dot", "application/msword");
            _mappings.Add(".dotm", "application/vnd.ms-word.template.macroEnabled.12");
            _mappings.Add(".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
            _mappings.Add(".dsp", "application/octet-stream");
            _mappings.Add(".dtd", "text/xml");
            _mappings.Add(".dvi", "application/x-dvi");
            _mappings.Add(".dvr-ms", "video/x-ms-dvr");
            _mappings.Add(".dwf", "drawing/x-dwf");
            _mappings.Add(".dwp", "application/octet-stream");
            _mappings.Add(".dxr", "application/x-director");
            _mappings.Add(".eml", "message/rfc822");
            _mappings.Add(".emz", "application/octet-stream");
            _mappings.Add(".eot", "application/vnd.ms-fontobject");
            _mappings.Add(".eps", "application/postscript");
            _mappings.Add(".etx", "text/x-setext");
            _mappings.Add(".evy", "application/envoy");
            _mappings.Add(".fdf", "application/vnd.fdf");
            _mappings.Add(".fif", "application/fractals");
            _mappings.Add(".fla", "application/octet-stream");
            _mappings.Add(".flr", "x-world/x-vrml");
            _mappings.Add(".flv", "video/x-flv");
            _mappings.Add(".gif", "image/gif");
            _mappings.Add(".gtar", "application/x-gtar");
            _mappings.Add(".gz", "application/x-gzip");
            _mappings.Add(".h", "text/plain");
            _mappings.Add(".hdf", "application/x-hdf");
            _mappings.Add(".hdml", "text/x-hdml");
            _mappings.Add(".hhc", "application/x-oleobject");
            _mappings.Add(".hhk", "application/octet-stream");
            _mappings.Add(".hhp", "application/octet-stream");
            _mappings.Add(".hlp", "application/winhlp");
            _mappings.Add(".hqx", "application/mac-binhex40");
            _mappings.Add(".hta", "application/hta");
            _mappings.Add(".htc", "text/x-component");
            _mappings.Add(".htm", "text/html");
            _mappings.Add(".html", "text/html");
            _mappings.Add(".htt", "text/webviewhtml");
            _mappings.Add(".hxt", "text/html");
            _mappings.Add(".ical", "text/calendar");
            _mappings.Add(".icalendar", "text/calendar");
            _mappings.Add(".ico", "image/x-icon");
            _mappings.Add(".ics", "text/calendar");
            _mappings.Add(".ief", "image/ief");
            _mappings.Add(".ifb", "text/calendar");
            _mappings.Add(".iii", "application/x-iphone");
            _mappings.Add(".inf", "application/octet-stream");
            _mappings.Add(".ins", "application/x-internet-signup");
            _mappings.Add(".isp", "application/x-internet-signup");
            _mappings.Add(".IVF", "video/x-ivf");
            _mappings.Add(".jar", "application/java-archive");
            _mappings.Add(".java", "application/octet-stream");
            _mappings.Add(".jck", "application/liquidmotion");
            _mappings.Add(".jcz", "application/liquidmotion");
            _mappings.Add(".jfif", "image/pjpeg");
            _mappings.Add(".jpb", "application/octet-stream");
            _mappings.Add(".jpe", "image/jpeg");
            _mappings.Add(".jpeg", "image/jpeg");
            _mappings.Add(".jpg", "image/jpeg");
            _mappings.Add(".js", "application/javascript");
            _mappings.Add(".json", "application/json");
            _mappings.Add(".jsx", "text/jscript");
            _mappings.Add(".latex", "application/x-latex");
            _mappings.Add(".lit", "application/x-ms-reader");
            _mappings.Add(".lpk", "application/octet-stream");
            _mappings.Add(".lsf", "video/x-la-asf");
            _mappings.Add(".lsx", "video/x-la-asf");
            _mappings.Add(".lzh", "application/octet-stream");
            _mappings.Add(".m13", "application/x-msmediaview");
            _mappings.Add(".m14", "application/x-msmediaview");
            _mappings.Add(".m1v", "video/mpeg");
            _mappings.Add(".m2ts", "video/vnd.dlna.mpeg-tts");
            _mappings.Add(".m3u", "audio/x-mpegurl");
            _mappings.Add(".m4a", "audio/mp4");
            _mappings.Add(".m4v", "video/mp4");
            _mappings.Add(".man", "application/x-troff-man");
            _mappings.Add(".manifest", "application/x-ms-manifest");
            _mappings.Add(".map", "text/plain");
            _mappings.Add(".mdb", "application/x-msaccess");
            _mappings.Add(".mdp", "application/octet-stream");
            _mappings.Add(".me", "application/x-troff-me");
            _mappings.Add(".mht", "message/rfc822");
            _mappings.Add(".mhtml", "message/rfc822");
            _mappings.Add(".mid", "audio/mid");
            _mappings.Add(".midi", "audio/mid");
            _mappings.Add(".mix", "application/octet-stream");
            _mappings.Add(".mmf", "application/x-smaf");
            _mappings.Add(".mno", "text/xml");
            _mappings.Add(".mny", "application/x-msmoney");
            _mappings.Add(".mov", "video/quicktime");
            _mappings.Add(".movie", "video/x-sgi-movie");
            _mappings.Add(".mp2", "video/mpeg");
            _mappings.Add(".mp3", "audio/mpeg");
            _mappings.Add(".mp4", "video/mp4");
            _mappings.Add(".mp4v", "video/mp4");
            _mappings.Add(".mpa", "video/mpeg");
            _mappings.Add(".mpe", "video/mpeg");
            _mappings.Add(".mpeg", "video/mpeg");
            _mappings.Add(".mpg", "video/mpeg");
            _mappings.Add(".mpp", "application/vnd.ms-project");
            _mappings.Add(".mpv2", "video/mpeg");
            _mappings.Add(".ms", "application/x-troff-ms");
            _mappings.Add(".msi", "application/octet-stream");
            _mappings.Add(".mso", "application/octet-stream");
            _mappings.Add(".mvb", "application/x-msmediaview");
            _mappings.Add(".mvc", "application/x-miva-compiled");
            _mappings.Add(".nc", "application/x-netcdf");
            _mappings.Add(".nsc", "video/x-ms-asf");
            _mappings.Add(".nws", "message/rfc822");
            _mappings.Add(".ocx", "application/octet-stream");
            _mappings.Add(".oda", "application/oda");
            _mappings.Add(".odc", "text/x-ms-odc");
            _mappings.Add(".ods", "application/oleobject");
            _mappings.Add(".oga", "audio/ogg");
            _mappings.Add(".ogg", "video/ogg");
            _mappings.Add(".ogv", "video/ogg");
            _mappings.Add(".ogx", "application/ogg");
            _mappings.Add(".one", "application/onenote");
            _mappings.Add(".onea", "application/onenote");
            _mappings.Add(".onetoc", "application/onenote");
            _mappings.Add(".onetoc2", "application/onenote");
            _mappings.Add(".onetmp", "application/onenote");
            _mappings.Add(".onepkg", "application/onenote");
            _mappings.Add(".osdx", "application/opensearchdescription+xml");
            _mappings.Add(".otf", "font/otf");
            _mappings.Add(".p10", "application/pkcs10");
            _mappings.Add(".p12", "application/x-pkcs12");
            _mappings.Add(".p7b", "application/x-pkcs7-certificates");
            _mappings.Add(".p7c", "application/pkcs7-mime");
            _mappings.Add(".p7m", "application/pkcs7-mime");
            _mappings.Add(".p7r", "application/x-pkcs7-certreqresp");
            _mappings.Add(".p7s", "application/pkcs7-signature");
            _mappings.Add(".pbm", "image/x-portable-bitmap");
            _mappings.Add(".pcx", "application/octet-stream");
            _mappings.Add(".pcz", "application/octet-stream");
            _mappings.Add(".pdf", "application/pdf");
            _mappings.Add(".pfb", "application/octet-stream");
            _mappings.Add(".pfm", "application/octet-stream");
            _mappings.Add(".pfx", "application/x-pkcs12");
            _mappings.Add(".pgm", "image/x-portable-graymap");
            _mappings.Add(".pko", "application/vnd.ms-pki.pko");
            _mappings.Add(".pma", "application/x-perfmon");
            _mappings.Add(".pmc", "application/x-perfmon");
            _mappings.Add(".pml", "application/x-perfmon");
            _mappings.Add(".pmr", "application/x-perfmon");
            _mappings.Add(".pmw", "application/x-perfmon");
            _mappings.Add(".png", "image/png");
            _mappings.Add(".pnm", "image/x-portable-anymap");
            _mappings.Add(".pnz", "image/png");
            _mappings.Add(".pot", "application/vnd.ms-powerpoint");
            _mappings.Add(".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12");
            _mappings.Add(".potx", "application/vnd.openxmlformats-officedocument.presentationml.template");
            _mappings.Add(".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12");
            _mappings.Add(".ppm", "image/x-portable-pixmap");
            _mappings.Add(".pps", "application/vnd.ms-powerpoint");
            _mappings.Add(".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12");
            _mappings.Add(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
            _mappings.Add(".ppt", "application/vnd.ms-powerpoint");
            _mappings.Add(".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12");
            _mappings.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            _mappings.Add(".prf", "application/pics-rules");
            _mappings.Add(".prm", "application/octet-stream");
            _mappings.Add(".prx", "application/octet-stream");
            _mappings.Add(".ps", "application/postscript");
            _mappings.Add(".psd", "application/octet-stream");
            _mappings.Add(".psm", "application/octet-stream");
            _mappings.Add(".psp", "application/octet-stream");
            _mappings.Add(".pub", "application/x-mspublisher");
            _mappings.Add(".qt", "video/quicktime");
            _mappings.Add(".qtl", "application/x-quicktimeplayer");
            _mappings.Add(".qxd", "application/octet-stream");
            _mappings.Add(".ra", "audio/x-pn-realaudio");
            _mappings.Add(".ram", "audio/x-pn-realaudio");
            _mappings.Add(".rar", "application/octet-stream");
            _mappings.Add(".ras", "image/x-cmu-raster");
            _mappings.Add(".rf", "image/vnd.rn-realflash");
            _mappings.Add(".rgb", "image/x-rgb");
            _mappings.Add(".rm", "application/vnd.rn-realmedia");
            _mappings.Add(".rmi", "audio/mid");
            _mappings.Add(".roff", "application/x-troff");
            _mappings.Add(".rpm", "audio/x-pn-realaudio-plugin");
            _mappings.Add(".rtf", "application/rtf");
            _mappings.Add(".rtx", "text/richtext");
            _mappings.Add(".scd", "application/x-msschedule");
            _mappings.Add(".sct", "text/scriptlet");
            _mappings.Add(".sea", "application/octet-stream");
            _mappings.Add(".setpay", "application/set-payment-initiation");
            _mappings.Add(".setreg", "application/set-registration-initiation");
            _mappings.Add(".sgml", "text/sgml");
            _mappings.Add(".sh", "application/x-sh");
            _mappings.Add(".shar", "application/x-shar");
            _mappings.Add(".sit", "application/x-stuffit");
            _mappings.Add(".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12");
            _mappings.Add(".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide");
            _mappings.Add(".smd", "audio/x-smd");
            _mappings.Add(".smi", "application/octet-stream");
            _mappings.Add(".smx", "audio/x-smd");
            _mappings.Add(".smz", "audio/x-smd");
            _mappings.Add(".snd", "audio/basic");
            _mappings.Add(".snp", "application/octet-stream");
            _mappings.Add(".spc", "application/x-pkcs7-certificates");
            _mappings.Add(".spl", "application/futuresplash");
            _mappings.Add(".spx", "audio/ogg");
            _mappings.Add(".src", "application/x-wais-source");
            _mappings.Add(".ssm", "application/streamingmedia");
            _mappings.Add(".sst", "application/vnd.ms-pki.certstore");
            _mappings.Add(".stl", "application/vnd.ms-pki.stl");
            _mappings.Add(".sv4cpio", "application/x-sv4cpio");
            _mappings.Add(".sv4crc", "application/x-sv4crc");
            _mappings.Add(".svg", "image/svg+xml");
            _mappings.Add(".svgz", "image/svg+xml");
            _mappings.Add(".swf", "application/x-shockwave-flash");
            _mappings.Add(".t", "application/x-troff");
            _mappings.Add(".tar", "application/x-tar");
            _mappings.Add(".tcl", "application/x-tcl");
            _mappings.Add(".tex", "application/x-tex");
            _mappings.Add(".texi", "application/x-texinfo");
            _mappings.Add(".texinfo", "application/x-texinfo");
            _mappings.Add(".tgz", "application/x-compressed");
            _mappings.Add(".thmx", "application/vnd.ms-officetheme");
            _mappings.Add(".thn", "application/octet-stream");
            _mappings.Add(".tif", "image/tiff");
            _mappings.Add(".tiff", "image/tiff");
            _mappings.Add(".toc", "application/octet-stream");
            _mappings.Add(".tr", "application/x-troff");
            _mappings.Add(".trm", "application/x-msterminal");
            _mappings.Add(".ts", "video/vnd.dlna.mpeg-tts");
            _mappings.Add(".tsv", "text/tab-separated-values");
            _mappings.Add(".ttf", "application/octet-stream");
            _mappings.Add(".tts", "video/vnd.dlna.mpeg-tts");
            _mappings.Add(".txt", "text/plain");
            _mappings.Add(".u32", "application/octet-stream");
            _mappings.Add(".uls", "text/iuls");
            _mappings.Add(".ustar", "application/x-ustar");
            _mappings.Add(".vbs", "text/vbscript");
            _mappings.Add(".vcf", "text/x-vcard");
            _mappings.Add(".vcs", "text/plain");
            _mappings.Add(".vdx", "application/vnd.ms-visio.viewer");
            _mappings.Add(".vml", "text/xml");
            _mappings.Add(".vsd", "application/vnd.visio");
            _mappings.Add(".vss", "application/vnd.visio");
            _mappings.Add(".vst", "application/vnd.visio");
            _mappings.Add(".vsto", "application/x-ms-vsto");
            _mappings.Add(".vsw", "application/vnd.visio");
            _mappings.Add(".vsx", "application/vnd.visio");
            _mappings.Add(".vtx", "application/vnd.visio");
            _mappings.Add(".wav", "audio/wav");
            _mappings.Add(".wax", "audio/x-ms-wax");
            _mappings.Add(".wbmp", "image/vnd.wap.wbmp");
            _mappings.Add(".wcm", "application/vnd.ms-works");
            _mappings.Add(".wdb", "application/vnd.ms-works");
            _mappings.Add(".webm", "video/webm");
            _mappings.Add(".wks", "application/vnd.ms-works");
            _mappings.Add(".wm", "video/x-ms-wm");
            _mappings.Add(".wma", "audio/x-ms-wma");
            _mappings.Add(".wmd", "application/x-ms-wmd");
            _mappings.Add(".wmf", "application/x-msmetafile");
            _mappings.Add(".wml", "text/vnd.wap.wml");
            _mappings.Add(".wmlc", "application/vnd.wap.wmlc");
            _mappings.Add(".wmls", "text/vnd.wap.wmlscript");
            _mappings.Add(".wmlsc", "application/vnd.wap.wmlscriptc");
            _mappings.Add(".wmp", "video/x-ms-wmp");
            _mappings.Add(".wmv", "video/x-ms-wmv");
            _mappings.Add(".wmx", "video/x-ms-wmx");
            _mappings.Add(".wmz", "application/x-ms-wmz");
            _mappings.Add(".woff", "application/font-woff");
            _mappings.Add(".wps", "application/vnd.ms-works");
            _mappings.Add(".wri", "application/x-mswrite");
            _mappings.Add(".wrl", "x-world/x-vrml");
            _mappings.Add(".wrz", "x-world/x-vrml");
            _mappings.Add(".wsdl", "text/xml");
            _mappings.Add(".wtv", "video/x-ms-wtv");
            _mappings.Add(".wvx", "video/x-ms-wvx");
            _mappings.Add(".x", "application/directx");
            _mappings.Add(".xaf", "x-world/x-vrml");
            _mappings.Add(".xaml", "application/xaml+xml");
            _mappings.Add(".xap", "application/x-silverlight-app");
            _mappings.Add(".xbap", "application/x-ms-xbap");
            _mappings.Add(".xbm", "image/x-xbitmap");
            _mappings.Add(".xdr", "text/plain");
            _mappings.Add(".xht", "application/xhtml+xml");
            _mappings.Add(".xhtml", "application/xhtml+xml");
            _mappings.Add(".xla", "application/vnd.ms-excel");
            _mappings.Add(".xlam", "application/vnd.ms-excel.addin.macroEnabled.12");
            _mappings.Add(".xlc", "application/vnd.ms-excel");
            _mappings.Add(".xlm", "application/vnd.ms-excel");
            _mappings.Add(".xls", "application/vnd.ms-excel");
            _mappings.Add(".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12");
            _mappings.Add(".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12");
            _mappings.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            _mappings.Add(".xlt", "application/vnd.ms-excel");
            _mappings.Add(".xltm", "application/vnd.ms-excel.template.macroEnabled.12");
            _mappings.Add(".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
            _mappings.Add(".xlw", "application/vnd.ms-excel");
            _mappings.Add(".xml", "text/xml");
            _mappings.Add(".xof", "x-world/x-vrml");
            _mappings.Add(".xpm", "image/x-xpixmap");
            _mappings.Add(".xps", "application/vnd.ms-xpsdocument");
            _mappings.Add(".xsd", "text/xml");
            _mappings.Add(".xsf", "text/xml");
            _mappings.Add(".xsl", "text/xml");
            _mappings.Add(".xslt", "text/xml");
            _mappings.Add(".xsn", "application/octet-stream");
            _mappings.Add(".xtp", "application/octet-stream");
            _mappings.Add(".xwd", "image/x-xwindowdump");
            _mappings.Add(".z", "application/x-compress");
            _mappings.Add(".zip", "application/x-zip-compressed");
        }


        private readonly Dictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);


        /// <summary>
        /// Возвращает фомат данных файла по его наименованию.
        /// </summary>
        /// <param name="blobName">Наименование файла.</param>
        /// <returns>Формат данных файла.</returns>
        public string GetBlobType(string blobName)
        {
            string blobType = null;

            if (!string.IsNullOrEmpty(blobName))
            {
                var extension = Path.GetExtension(blobName);

                if (!string.IsNullOrEmpty(blobName))
                {
                    _mappings.TryGetValue(extension, out blobType);
                }
            }

            if (string.IsNullOrEmpty(blobType))
            {
                blobType = "application/octet-stream";
            }

            return blobType;
        }
    }
}