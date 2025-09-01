using OpenQA.Selenium;
using ChallengeQa.Components;

public class RecoverPasswordPage
{
    private readonly MessageComponent _titulo;
    private readonly MessageComponent _mensagem;

    public RecoverPasswordPage(IWebDriver driver)
    {
        _titulo = new MessageComponent(driver, By.TagName("h3"));
        _mensagem = new MessageComponent(driver, By.CssSelector("p.text-muted-foreground"));
    }

    public string ObterTitulo() => _titulo.GetText();
    public string ObterMensagem() => _mensagem.GetText();
}
