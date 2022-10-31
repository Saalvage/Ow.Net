namespace Ow.Net.Data; 

public class FullProfile
    : BaseProfile {

    internal FullProfile(string name, uint level, string profileUrl, string profilePictureUrl,
        Endorsement endorsement) : base(name, level, profileUrl, profilePictureUrl, endorsement, true) {

    }
}
