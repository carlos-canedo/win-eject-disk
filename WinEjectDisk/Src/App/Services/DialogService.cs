using WinEjectDisk.Src.App.Contracts;

namespace WinEjectDisk.Src.App.Services;

public sealed class DialogService : IDialogService
{
    public DialogResult ShowError(string title, string message)
    {
        return MessageBox.Show(
            text: message,
            caption: title,
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    public DialogResult ShowInfo(string title, string message)
    {
        return MessageBox.Show(
            text: message,
            caption: title,
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}
