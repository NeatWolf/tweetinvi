﻿using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.AccountSettings;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AccountSettingsClient
{
    public class AccountSettingsQueryExecutorTests
    {
        public AccountSettingsQueryExecutorTests()
        {
            _fakeBuilder = new FakeClassBuilder<AccountSettingsQueryExecutor>();
            _fakeAccountSettingsQueryGenerator = _fakeBuilder.GetFake<IAccountSettingsQueryGenerator>().FakedObject;
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        private readonly FakeClassBuilder<AccountSettingsQueryExecutor> _fakeBuilder;
        private readonly IAccountSettingsQueryGenerator _fakeAccountSettingsQueryGenerator;
        private readonly ITwitterAccessor _fakeTwitterAccessor;

        private AccountSettingsQueryExecutor CreateAccountSettingsQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
        
        [Fact]
        public async Task GetAccountSettings_ReturnsAccountUserDTO()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new GetAccountSettingsParameters();

            var query = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IAccountSettingsDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetAccountSettingsQuery(parameters)).Returns(query);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<IAccountSettingsDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.GetAccountSettings(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(HttpMethod.GET, request.Query.HttpMethod);
        }
        
        [Fact]
        public async Task UpdateAccountSettings_ReturnsAccountUserDTO()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new UpdateAccountSettingsParameters();

            var query = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IAccountSettingsDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetUpdateAccountSettingsQuery(parameters)).Returns(query);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<IAccountSettingsDTO>(request)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UpdateAccountSettings(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }

        [Fact]
        public async Task UpdateProfileImage_ReturnsAccountUserDTO()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new UpdateProfileImageParameters(null);

            var query = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetUpdateProfileImageQuery(parameters)).Returns(query);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<IUserDTO>(A<ITwitterRequest>
                    .That.Matches(x => x.Query is IMultipartTwitterQuery && x.Query.Url == query)))
                .Returns(expectedResult);

            // Act
            var result = await queryExecutor.UpdateProfileImage(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }
        
        [Fact]
        public async Task UpdateProfileBanner_ReturnsAccountUserDTO()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new UpdateProfileBannerParameters(new byte[2]);

            var query = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetUpdateProfileBannerQuery(parameters)).Returns(query);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest(A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.UpdateProfileBanner(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.True(request.Query.IsHttpContentPartOfQueryParams);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }
        
        [Fact]
        public async Task RemoveProfileBanner_ReturnsAccountUserDTO()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new RemoveProfileBannerParameters();

            var query = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetRemoveProfileBannerQuery(parameters)).Returns(query);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest(A<ITwitterRequest>.Ignored)).Returns(expectedResult);

            // Act
            var result = await queryExecutor.RemoveProfileBanner(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
            Assert.Equal(HttpMethod.POST, request.Query.HttpMethod);
        }
    }
}