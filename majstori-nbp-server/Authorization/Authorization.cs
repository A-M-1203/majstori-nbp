namespace majstori_nbp_server.Authorization;

public class Authorization
{
    public static bool IsMajstor(string role)
    {
        return role == "majstor";
    }
    public static bool IsKorisnik(string role)
    {
        return role == "korisnik";
    }
}