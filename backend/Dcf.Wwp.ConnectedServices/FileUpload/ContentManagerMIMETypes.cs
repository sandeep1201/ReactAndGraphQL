#region Namespaces

#endregion

namespace Dcf.Wwp.ConnectedServices.Documents
{
    public class ContentManagerMIMETypes
    {
        public string GetMIMEType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case "doc":
                case "dot":
                    return "application/msword";
                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "docm":
                    return "application/vnd.ms-word.document.macroEnabled.12";
                case "xls":
                case "xlt":
                case "xla":
                case "xlm":
                case "xld":
                case "xlc":
                case "xlw":
                case "xll":
                    return "application/vnd.ms-excel";
                case "xlsx":
                case "xlsm":
                case "xlsb":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case "ppt":
                case "pot":
                case "pps":
                case "ppa":
                case "pptx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case "tiff":
                case "tif":
                    return "image/tiff";
                case "jpg":
                case "jfif":
                case "pjpeg":
                case "jpe":
                case "pjp":
                case "jpeg":     
                    return "image/jpeg";
                case "bmp":
                    return "image/bmp";
                case "gif":
                    return "image/gif";
                case "png":
                    return "image/png";
                case "txt":
                case "text":
                case "rft":
                case "rtf":
                    return "text/plain";
                case "htm":
                case "html":
                    case "shtml":
                    return "text/html";
                case "xml":
                    return "text/xml";
                case "pdf":
                    return "application/pdf";
                case "zip":
                    return "application/zip";
                case "mid":
                case "midi":
                    return "audio/midi";
                case "aif":
                case "aifc":
                    return "audio/x-aiff";
                case "wav":
                case "kar":
                case "smf":
                    return "audio/x-wav";
                case "avi":
                case "mpeg":
                case "mpg":
                case "m1s":
                case "m1a":
                case "mp2":
                case "mpm":
                case "mpa":
                case "qcp":
                case "aiff":
                case "m1v":
                case "m75":
                case "m15":
                case "mov":
                case "qt":
                case "asf":
                case "asx":
                case "vfw":
                    return "video/avi";
                case "bin":
                case "fm":
                case "frm":
                case "mda":
                case "123":
                case "wk4":
                case "wk3":
                case "wk1":
                case "wks":
                case "wg1":
                case "prz":
                case "pre":
                case "lwp":
                case "sam":
                case "mwp":
                case "smm":
                case "pwz":
                case "vsd":
                case "wp":
                case "wpd":
                case "w51":
                case "au":
                case "snd":
                case "ulw":
                    return "application/octet-stream";
                default:
                    return "";

            }
        }
    }
}
