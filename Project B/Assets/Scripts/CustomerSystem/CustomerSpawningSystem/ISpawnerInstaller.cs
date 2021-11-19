public interface ISpawnerInstaller
{
    public void SetSpawnerPositionMethod(ISpawnerPositionMethod plannerMethod);
    public void SetNPCSelectorMethod(INPCSelectorMethod npcSelectorMethod);
    public void SetSpawnerMethod(ISpawnerMethod spawnerMethod);
}
