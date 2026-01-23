namespace WinEjectDisk.Src.App.Contracts;

public interface IDialogService
{
    DialogResult ShowInfo(string title, string message);
    DialogResult ShowError(string title, string message);
}
