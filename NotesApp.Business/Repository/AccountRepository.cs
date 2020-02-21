using NotesApp.Business.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NotesApp.Business
{
    public class AccountRepository : IAccountRepository
    {
        public User Login(LoginViewModel login, string ipAddress)
        {
            try
            {
                using (var db = new NotesAppEntities())
                {
                    var result = db.Users.FirstOrDefault(s => s.Email == login.Email && s.Password == login.Password);
                    result.IpAddress = ipAddress;
                    db.AuthenticationLogs.Add(
                        new AuthenticationLog
                        {
                            CreatedUTC = DateTime.UtcNow,
                            UserId = result.UserId,
                            LogType = LogTypeEnum.Login.ToString()
                        });
                    db.SaveChanges();
                    if (result != null)
                    {
                        return result;
                    }
                    else { return null; }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Register(User user)
        {
            try
            {
                using (var db = new NotesAppEntities())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckValidForTransaction(int UserId, string UserIp)
        {
            try
            {
                using (var db = new NotesAppEntities())
                {
                    var CheckForMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["CheckForMinutes"]);
                    var MaxLogCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLogCount"]);
                    var transactions = db.AuthenticationLogs.Where(s => s.User.IpAddress == UserIp && s.UserId == UserId).ToList();
                    var CountInOneMin = transactions.Where(s => s.CreatedUTC >= DateTime.UtcNow.AddMinutes(-CheckForMinutes)).ToList().Count;
                    if (CountInOneMin > MaxLogCount)
                    {
                        db.BLockedIps.Add(new BLockedIp { IpAddress = UserIp, UserId = UserId, CreatedUTC = DateTime.UtcNow });
                        db.SaveChanges();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BLockedIp> GetBlockedIpList()
        {
            try
            {
                using (var db = new NotesAppEntities())
                {
                    return db.BLockedIps.Include("User").OrderByDescending(s => s.CreatedUTC).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}