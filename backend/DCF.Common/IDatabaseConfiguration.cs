namespace DCF.Common
{
    public interface IDatabaseConfiguration
    {
        #region Properties

        string Server      { get; set; }
        string Catalog     { get; set; }
        string UserId      { get; set; }
        string Password    { get; set; }
        int    MaxPoolSize { get; set; }
        int    Timeout     { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
