namespace TgBotPractice;
using TelegramBotBase.Base;
using TelegramBotBase.Form;
public class OpenForm : FormBase
{
    
    public override async Task Render(MessageResult message)
    {
        await Device.Send("OpenComand");
    }
}