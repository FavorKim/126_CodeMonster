using UnityEditor.Build.Player;

public interface ICommand
{
    void Execute(Player player);
}