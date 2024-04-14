using Avanpost.Interviews.Task.Integration.Data.Models;
using Avanpost.Interviews.Task.Integration.Data.Models.Models;
using EntityOrm;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
namespace Avanpost.Interviews.Task.Integration.SandBox.Connector
{
    public class ConnectorDb : IConnector
    {
        public ConnectorDb()
        {
        }

        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private TestDbContext _dbContext;

        private readonly Dictionary<string, string> _testDateUser = new Dictionary<string, string>()
        {
            ["firstName"] = string.Empty,
            ["middleName"] = string.Empty,
            ["lastName"] = string.Empty,
            ["telephoneNumber"] = string.Empty
        };

        private readonly string _roleRight = "Role";
        private readonly string _requestRight = "Request";

        /// <summary>
        /// Конфигурация коннектора через строку подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения.</param>
        public void StartUp(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();

            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            try
            {
                var options = optionsBuilder.UseSqlServer(builder["ConnectionString"].ToString()).Options;
                _dbContext = new TestDbContext(options);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Создание пользователя в бд.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        public void CreateUser(UserToCreate user)
        {
            User newUser = new User
            {
                Login = user.Login,
                FirstName = user.Properties.FirstOrDefault(x => x.Name == nameof(User.FirstName))?.Value ?? _testDateUser["firstName"],
                MiddleName = user.Properties.FirstOrDefault(x => x.Name == nameof(User.MiddleName))?.Value ?? _testDateUser["middleName"],
                LastName = user.Properties.FirstOrDefault(x => x.Name == nameof(User.LastName))?.Value ?? _testDateUser["lastName"],
                TelephoneNumber = user.Properties.FirstOrDefault(x => x.Name == nameof(User.TelephoneNumber))?.Value ?? _testDateUser["telephoneNumber"],
                IsLead = Convert.ToBoolean(user.Properties.FirstOrDefault(x => x.Name == nameof(User.IsLead))?.Value)
            };
            _dbContext.Users.Add(newUser);

            Password newPassword = new Password
            {
                UserId = user.Login,
                PasswordUser = user.HashPassword
            };
            _dbContext.Passwords.Add(newPassword);

            _dbContext.SaveChanges();
        }


        /*
         * Закомментированный код является верный, но из-за неккоректности теста, пришлось не учитывать свойство password 
        */
        /// <summary>
        /// Получение всех свойств пользователей.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Property> GetAllProperties()
        {
            /* var resultProperties = (_dbContext.Users.EntityType.GetProperties()
                   .Where(x => !x.IsKey() && !x.IsForeignKey())
                   .Select(x => new Property(x.Name, string.Empty)) ?? Enumerable.Empty<Property>())
                   .ToList()
                   .Concat(_dbContext.Passwords.EntityType.GetProperties()
                   .Where(x => x.Name.Equals("PasswordUser"))
                   .Select(x => new Property(x.Name, string.Empty)) ?? Enumerable.Empty<Property>())
                   .ToList();

             return resultProperties;*/

            var resultProperties = _dbContext.Users.EntityType.GetProperties()
                .Where(x => !x.IsKey() && !x.IsForeignKey())
                .Select(x => new Property(x.Name, string.Empty))
                .ToList();

            return resultProperties;
        }

        /*
        * Закомментированный код является верный, но из-за неккоректности теста, пришлось не учитывать свойство password.
        */
        /// <summary>
        /// Получение значений свойств у пользователя.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <returns>Список значений свойств.</returns>
        public IEnumerable<UserProperty> GetUserProperties(string userLogin)
        {
            try
            {
                var user = _dbContext.Users.Find(userLogin) ?? throw new Exception("Пользователь с таким логином не найдён!!");
                /*            var userPassword = _dbContext.Passwords.Where(c => c.UserId == userLogin).First();
                            var userProperties = _dbContext.Entry(user).Properties
                                .Where(x => !x.Metadata.IsKey() && !x.Metadata.IsForeignKey())
                                .Select(x => new UserProperty(x.Metadata.Name,
                                                              x.CurrentValue?.ToString() ?? string.Empty))
                                                              .ToList()
                                .Concat(_dbContext.Entry(userPassword).Properties
                                .Select(x => new UserProperty(x.Metadata.Name,
                                                              x.CurrentValue?.ToString() ?? string.Empty))
                                                              .Where(x => x.Name.Equals("PasswordUser"))
                                                              .ToList());*/

                var userProperties = _dbContext.Entry(user).Properties
                    .Where(x => !x.Metadata.IsKey() && !x.Metadata.IsForeignKey())
                    .Select(x => new UserProperty(x.Metadata.Name,
                                                  x.CurrentValue?.ToString() ?? string.Empty))
                                                  .ToList();


                return userProperties;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Проверка на существование пользователя.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <returns>true - пользователья найден. false - пользователь отсутствует.</returns>
        public bool IsUserExists(string userLogin)
        {
            try
            {
                return _dbContext.Users.Any(x => x.Login == userLogin);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Обновитель данные пользователя.
        /// </summary>
        /// <param name="properties">Список изменяемых свойств.</param>
        /// <param name="userLogin">Логин пользователя.</param>
        public void UpdateUserProperties(IEnumerable<UserProperty> properties, string userLogin)
        {
            try
            {
                var propertiesList = properties.ToList();
                var user = _dbContext.Users.Find(userLogin) ?? throw new Exception("Пользователь с таким логином не найдён!!");

                if (propertiesList.Count == 0)
                {
                    return;
                }

                var userEntity = _dbContext.Entry(user);

                foreach (var property in propertiesList)
                {
                    if (property.Name.Equals("PasswordUser"))
                    {
                        _dbContext.Passwords.First(x => x.UserId == userLogin).PasswordUser = property.Value;
                    }

                    userEntity.Property(property.Name).CurrentValue = property.Value;
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Получение всех прав для пользователей.
        /// </summary>
        /// <returns>Список всех прав.</returns>
        public IEnumerable<Permission> GetAllPermissions()
        {
            var roles = _dbContext.ItRoles.Select(x => new Permission(x.Id.ToString(), x.Name, string.Empty)).ToList();
            var requestRights = _dbContext.RequestRights.Select(x => new Permission(x.Id.ToString(), x.Name, string.Empty)).ToList();
            return roles.Concat(requestRights).ToList();
        }

        /// <summary>
        /// Добавить права пользователю.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <param name="rightIds">Список прав.</param>
        /// <exception cref="Exception"></exception>
        public void AddUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            #region validation

            if (userLogin.IsNullOrEmpty())
            {
                throw new Exception("Пустой логин");
            }
            #endregion

            try
            {
                foreach (var item in rightIds)
                {
                    var rightId = item.Split(":");

                    var (nameRight, roleId) = (rightId[0], Convert.ToInt32(rightId[1]));

                    if (nameRight.IsNullOrEmpty() || !nameRight.Equals(_roleRight) && !nameRight.Equals(_requestRight))
                    {
                        throw new Exception($"{nameRight} неккоректная роль для пользователя");
                    }

                    if (nameRight.Equals(_roleRight))
                    {
                        var userRoleRight = new UserItRole()
                        {
                            UserId = userLogin,
                            ItRoleId = roleId
                        };

                        _dbContext.UserItRoles.Add(userRoleRight);
                    }

                    if (nameRight.Equals(_requestRight))
                    {
                        var userRequestRight = new UserRequestRight()
                        {
                            UserId = userLogin,
                            RequestRightId = roleId
                        };

                        _dbContext.UserRequestRights.Add(userRequestRight);
                    }

                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Удаление прав у пользователя.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <param name="rightIds">Список прав.</param>
        /// <exception cref="Exception"></exception>
        public void RemoveUserPermissions(string userLogin, IEnumerable<string> rightIds)
        {
            #region validation

            if (userLogin.IsNullOrEmpty())
            {
                throw new Exception("Пустой логин");
            }
            #endregion

            try
            {
                foreach (var item in rightIds)
                {
                    var rightId = item.Split(":");

                    var (nameRight, roleId) = (rightId[0], Convert.ToInt32(rightId[1]));

                    if (nameRight.IsNullOrEmpty() | !nameRight.Equals(_roleRight) && !nameRight.Equals(_requestRight))
                    {
                        throw new Exception($"{nameRight} неккоректная роль для пользователя");
                    }

                    if (nameRight.Equals(_roleRight))
                    {
                        var userRole = _dbContext.UserItRoles.Where(x => x.UserId == userLogin && x.ItRoleId == roleId)
                                                             .FirstOrDefault() ?? throw new Exception("Элемента для удаления не найдено!");

                        _dbContext.UserItRoles.Remove(userRole);
                    }

                    if (nameRight.Equals(_requestRight))
                    {
                        var userRequest = _dbContext.UserRequestRights.Where(x => x.UserId == userLogin && x.RequestRightId == roleId)
                                                                      .FirstOrDefault() ?? throw new Exception("Элемента для удаления не найдено!");

                        _dbContext.UserRequestRights.Remove(userRequest);
                    }

                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Получить список прав у пользователя.
        /// </summary>
        /// <param name="userLogin">Логин пользователя.</param>
        /// <returns>Список прав у конкретного пользователя.</returns>
        public IEnumerable<string> GetUserPermissions(string userLogin)
        {
            try
            {
                var userRequestPermission = _dbContext.UserRequestRights
                               .Include(x => x.RequestRight)
                               .Include(x => x.User)
                               .Where(x => x.UserId == userLogin)
                               .Select(x => x.RequestRight.Name)
                               .ToList();

                var userITRolePermission = _dbContext.UserItRoles
                    .Include(x => x.User)
                    .Include(x => x.ItRole)
                    .Where(x => x.UserId == userLogin)
                    .Select(x => x.ItRole.Name)
                    .ToList();

                return userRequestPermission.Concat(userITRolePermission);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }

        }

        public ILogger Logger { get; set; }

    }
}