using TelegramBotBase.Base;
using TelegramBotBase.Form;
using TelegramBotBase.Enums;

namespace TgBotPractice;

public class StartForm : AutoCleanForm
{
    public StartForm()
    {
        DeleteMode = EDeleteMode.OnLeavingForm;
        Init += Start_Init;
        Opened += Start_Opened;
        Closed += Start_Closed;
        
    }

    // Gets invoked on initialization, before navigation
    private async Task Start_Init(object sender, EventArgs e)
    {
    }

    // Gets invoked after opened
    private async Task Start_Opened(object sender, EventArgs e)
    {
    }

    // Gets invoked after form has been closed
    private async Task Start_Closed(object sender, EventArgs e)
    {
    }

    // Gets invoked during Navigation to this form
    public override async Task PreLoad(MessageResult message)
    {
    }

    // Gets invoked on every Message/Action/Data in this context
    public override async Task Load(MessageResult message)
    {
    }

    // Gets invoked on edited messages
    public override async Task Edited(MessageResult message)
    {
    }

    // Gets invoked on Button clicks
    public override async Task Action(MessageResult message)
    {
    }

    // Gets invoked on Data uploades by the user (of type Photo, Audio, Video, Contact, Location, Document)
    public override async Task SentData(DataResult data)
    {
    }

    //Gets invoked on every Message/Action/Data to render Design or Response 
    public override async Task Render(MessageResult message)
    {
        await Device.Send("Привет! Это тестовый бот, который в будущем сможет показывать погоду в Самаре. Пока что есть тестовая команда: /open");
    }
}