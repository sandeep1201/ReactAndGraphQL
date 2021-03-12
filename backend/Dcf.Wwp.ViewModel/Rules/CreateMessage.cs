using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;

namespace Dcf.Wwp.Api.Library.Rules
{
    public static class CreateMessage
    {
        #region Properties

        #endregion

        #region Methods

        public static void CreateMsg(MessageCodeLevelContext messageCodeLevelResult, string code, CodeLevel codeLevel)
        {
            var msg = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, codeLevel);
        }

        public static void CreateMsg(CodeLevelMessageContext messageCodeLevelResult, string code, CodeLevel codeLevel)
        {
            var msg = messageCodeLevelResult.GetMessageByCode(code);
            messageCodeLevelResult.AddMessageCodeAndLevel(msg, code, codeLevel);
        }

        #endregion
    }
}
