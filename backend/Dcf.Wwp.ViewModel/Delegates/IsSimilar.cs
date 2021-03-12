namespace Dcf.Wwp.Api.Library.Delegates
{
    /// <summary>
    /// Method that takes two different types and if they are similar will adopt item1 to
    /// the identity of item2.  This can also work for complex/nested objects.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="item1"></param>
    /// <param name="item2"></param>
    /// <returns></returns>
    public delegate bool AdoptIfSimilar<in T1, in T2>(T1 item1, T2 item2);
}