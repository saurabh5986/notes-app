using NotesApp.Business.Models;
using System.Collections.Generic;

namespace NotesApp.Business
{
    public interface IAccountRepository
    {
        User Login(LoginViewModel login, string ip);

        bool Register(User user);

        bool CheckValidForTransaction(int UserId, string UserIp);

        List<BLockedIp> GetBlockedIpList();
    }
}