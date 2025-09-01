using OpenQA.Selenium;
using ChallengeQa.Components;

public class RecoverUsernamePage
{
    private readonly MessageComponent _titulo;
    private readonly MessageComponent _mensagem;
    private readonly ButtonComponent _voltar;

    public RecoverUsernamePage(IWebDriver driver)
    {
        _titulo = new MessageComponent(driver, By.TagName("h3"));
        _mensagem = new MessageComponent(driver, By.CssSelector("p.text-sm.text-muted-foreground"));
        _voltar = new ButtonComponent(driver, By.CssSelector("button"));
    }

    public string ObterTitulo() => _titulo.GetText();
    public string ObterMensagem() => _mensagem.GetText();
    public void VoltarParaHome() => _voltar.Clicar();
}
