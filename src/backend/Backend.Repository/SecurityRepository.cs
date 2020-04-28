using Backend.Core.Infrastructure;
using Backend.Helpers;
using Backend.Model;
using Backend.Repository.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Net;

namespace Backend.Repository
{
    /**
     * using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
{
  while (await reader.ReadAsync())
  {
    var data = await reader.GetTextReader(0).ReadToEndAsync();
  }
}
        **/
    public class SecurityRepository: BaseRepository<SecurityRepository>
    {
        public SecurityRepository(IOptions<RepoOptions> repoOptions, ILogger<SecurityRepository> logger)
            : base(repoOptions, logger)
        {
        }

        public void RegisterUserAsync(User user, Profile profile)
        {
            Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Entrance, $"user: {user} - profile: {profile}");
            Check.NotNull(user, nameof(user));
            Check.NotNull(profile, nameof(profile));

            using (var connection_ = new NpgsqlConnection(RepoOptions.PgsqlPassword))
            {
                Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"openning connection to database");
                connection_.Open();
                Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"openned connection to database");
                var transaction = connection_.BeginTransaction();

                try
                {
                    var createUserCommand = CreateStoreProceduceCmd("CALL create_user(@email_val, @password_val, @new_user_id, @error_message)");
                    createUserCommand.Connection = connection_;

                    var emailParam = createUserCommand.Parameters.AddWithValue("email_val", user.Email);
                    emailParam.Direction = System.Data.ParameterDirection.Input;

                    var pwdParam = createUserCommand.Parameters.AddWithValue("password_val",NpgsqlTypes.NpgsqlDbType.Varchar , user.Password);
                    pwdParam.Direction = System.Data.ParameterDirection.Input;

                    var uidParam = createUserCommand.Parameters.AddWithValue("new_user_id", 0);
                    uidParam.Direction = System.Data.ParameterDirection.InputOutput;

                    var errorMessageParam = createUserCommand.Parameters.AddWithValue("error_message", "");
                    errorMessageParam.Direction = System.Data.ParameterDirection.InputOutput;

                    /**
                     user_id_val profiles.user_id%TYPE,IN occupation_val profiles.occupation%TYPE,
                                         */
                    var createProfileCommand = CreateStoreProceduceCmd("CALL create_user_profile(@user_id_val,@occupation_val,@first_name_val,@last_name_val,@birth_val,@gender_val,@marital_status_val,@profile_id_val,@error_message)");
                    createProfileCommand.Connection = connection_;

                    var occupationParam = createProfileCommand.Parameters.AddWithValue("occupation_val", profile.Occupation);
                    occupationParam.Direction = System.Data.ParameterDirection.Input;

                    var firstNameParam = createProfileCommand.Parameters.AddWithValue("first_name_val", profile.FirstName);
                    firstNameParam.Direction = System.Data.ParameterDirection.Input;

                    var lastNameParam = createProfileCommand.Parameters.AddWithValue("last_name_val", profile.LastName);
                    lastNameParam.Direction = System.Data.ParameterDirection.Input;

                    var birthParam = createProfileCommand.Parameters.AddWithValue("birth_val", NpgsqlTypes.NpgsqlDbType.Date ,profile.Birth);
                    birthParam.Direction = System.Data.ParameterDirection.Input;

                    var genderParam = createProfileCommand.Parameters.AddWithValue("gender_val", profile.Gender.ToString());
                    genderParam.Direction = System.Data.ParameterDirection.Input;

                    var maritalStatusParam = createProfileCommand.Parameters.AddWithValue("marital_status_val", profile.MaritalStatus.ToString());
                    maritalStatusParam.Direction = System.Data.ParameterDirection.Input;

                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"executing create user command.");
                    createUserCommand.ExecuteNonQuery();
                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"executed create user command.");

                    var createdUserId = uidParam.Value;
                    var errorMessage = errorMessageParam.Value?.ToString().Trim();
                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"createdUserId: {createdUserId},errorMessage {errorMessage} ");

                    if (createdUserId == null || createdUserId.ToString() == "0" || errorMessage.Length > 0)
                    {
                        Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"Rolling back transaction and throwing exception");
                        throw new BackendException(Convert.ToInt32(HttpStatusCode.BadRequest), errorMessage);
                    }

                    var userIdParam = createProfileCommand.Parameters.AddWithValue("user_id_val", createdUserId);
                    userIdParam.Direction = System.Data.ParameterDirection.Input;

                    var profileIdParam = createProfileCommand.Parameters.AddWithValue("profile_id_val", 0);
                    profileIdParam.Direction = System.Data.ParameterDirection.InputOutput;

                    var errorMessageParam2 = createProfileCommand.Parameters.AddWithValue("error_message", "");
                    errorMessageParam2.Direction = System.Data.ParameterDirection.InputOutput;

                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"executing create profile command.");
                    createProfileCommand.ExecuteNonQuery();
                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"executed create profile command.");

                    var createdProfileId = uidParam.Value;
                    errorMessage = errorMessageParam.Value?.ToString().Trim();
                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"createdProfileId: {createdProfileId},errorMessage {errorMessage} ");

                    if (createdProfileId == null || createdProfileId.ToString() == "0" || errorMessage.Length > 0)
                    {
                        Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"Rolling back transaction and throwing exception");
                        transaction.Rollback();
                        throw new BackendException(Convert.ToInt32(HttpStatusCode.BadRequest), errorMessage);
                    }

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"Exception {exception}", LogLevel.Warning);
                    transaction.Rollback();

                    if (exception is BackendException)
                    {
                        throw exception;
                    }

                    throw new BackendException(Convert.ToInt32(HttpStatusCode.InternalServerError), exception.Message);
                }
            }

            Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Exit);
        }


        public bool ValidateUser(User user)
        {
            Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Entrance, $"user: {user}");
            Check.NotNull(user, nameof(user));

            using (var connection_ = new NpgsqlConnection(RepoOptions.PgsqlPassword))
            {
                try
                {
                    var validateUserCommand = CreateStoreProceduceCmd("CALL validate_user(@email_val, @password_val, @user_id_val, @error_message)");
                    validateUserCommand.Connection = connection_;

                    var emailParam = validateUserCommand.Parameters.AddWithValue("email_val", NpgsqlTypes.NpgsqlDbType.Varchar, user.Email);
                    emailParam.Direction = System.Data.ParameterDirection.Input;

                    var pwdParam = validateUserCommand.Parameters.AddWithValue("password_val", NpgsqlTypes.NpgsqlDbType.Varchar, user.Password);
                    pwdParam.Direction = System.Data.ParameterDirection.Input;

                    var uidParam = validateUserCommand.Parameters.AddWithValue("user_id_val", 0);
                    uidParam.Direction = System.Data.ParameterDirection.InputOutput;

                    var errorMessageParam = validateUserCommand.Parameters.AddWithValue("error_message", "");
                    errorMessageParam.Direction = System.Data.ParameterDirection.InputOutput;


                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"openning connection to database");
                    connection_.Open();
                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"openned connection to database");

                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"executing validate user command.");
                    validateUserCommand.ExecuteNonQuery();
                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"executed validate user command.");

                    var existingUserId = Convert.ToString(uidParam.Value) ;
                    var errorMessage = errorMessageParam.Value?.ToString().Trim();
                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"createdUserId: {existingUserId},errorMessage {errorMessage} ");

                    if ( String.IsNullOrEmpty(existingUserId) || existingUserId.ToString() == "0" || errorMessage.Length > 0)
                    {
                        Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"User with email: {user.Email} throwing exception");
                        throw new BackendException(Convert.ToInt32(HttpStatusCode.NotFound),String.IsNullOrEmpty(errorMessage) ? "Email and/or password invalid" : errorMessage);
                    }

                    user.Id = existingUserId;
                }
                catch (Exception exception)
                {
                    Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Body, $"Exception {exception}", LogLevel.Warning);

                    if (exception is BackendException)
                    {
                        throw exception;
                    }

                    throw new BackendException(Convert.ToInt32(HttpStatusCode.InternalServerError), exception.Message);
                }
            }

            Check.CallerLog<SecurityRepository>(Logger, LoggerExecutionPositions.Exit);
            return true; //for now  user does not need to enable account via email
        }
    }
}
