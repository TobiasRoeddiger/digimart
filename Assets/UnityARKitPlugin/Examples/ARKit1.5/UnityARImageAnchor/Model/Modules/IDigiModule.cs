public interface IDigiModule
{
    IDigiFilter Filter { get; }
    Form Form { get; }

    void ApplyForm();
}
