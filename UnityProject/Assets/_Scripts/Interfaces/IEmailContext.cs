
public interface IEmailContext
{
    void ChangeWriteState(HistoryPartState state);
    void ChangeResponseState(HistoryPartState state);
    void CheckApkSideStory(PointEmailKey sideEmailKey);

}

