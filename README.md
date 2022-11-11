# Demo for Ignite Spotlight on Korea #

Ignite Spotlight on Korea 행사에 사용할 데모 세션을 위한 리포지토리입니다.


## 사전 준비사항 ##

* [Bicep CLI](https://learn.microsoft.com/ko-kr/azure/azure-resource-manager/bicep/install)
* [Azure CLI](https://learn.microsoft.com/ko-kr/cli/azure/install-azure-cli)
* [GitHub CLI](https://cli.github.com/manual/installation)
* [Azure Developer CLI](https://learn.microsoft.com/ko-kr/azure/developer/azure-developer-cli/install-azd)
* [Azure Functions Core Tools](https://learn.microsoft.com/ko-kr/azure/azure-functions/functions-run-local)
* [Static Web App CLI](https://azure.github.io/static-web-apps-cli/docs/use/install)
* [PowerShell](https://learn.microsoft.com/ko-kr/powershell/scripting/install/installing-powershell)


## 시작하기 ##

1. 이 리포지토리를 자신의 깃헙 계정으로 포크합니다.

2. 깃헙 시크릿에 저장할 값들을 준비합니다.
   * [애저 퍼스널 액세스 토큰](https://github.com/Azure/login#configure-deployment-credentials) ➡️ `AZURE_CREDENTIALS`
   * [깃헙 퍼스널 액세스 토큰](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token) ➡️ `GH_ACCESS_TOKEN`
   * [네이버 맵 API 클라이언트 ID](https://api.ncloud-docs.com/docs/en/ai-naver-mapsstaticmap) ➡️ `MAPS_NAVER_CLIENTID`
   * [네이버 맵 API 클라이언트 시크릿](https://api.ncloud-docs.com/docs/en/ai-naver-mapsstaticmap) ➡️ `MAPS_NAVER_CLIENTSECRET`
   * [카카오 API 클라이언트 ID](https://developers.kakao.com/) ➡️ `KAKAO_CLIENTID`
   * [카카오 API 클라이언트 시크릿](https://developers.kakao.com/) ➡️ `KAKAO_CLIENTSECRET`
   * [NHN 클라우드 App Key](https://www.toast.com/kr) ➡️ `NHN_CLIENTID`
   * [NHN 클라우드 App Secret](https://www.toast.com/kr) ➡️ `NHN_CLIENTSECRET`

3. 터미널에서 아래 명령어를 입력한 후 지시를 따라 마무리합니다.

    ```bash
    # Azure Developer CLI: 초기화
    azd init
    ```

   이후 아래 명령어를 이용해 애플리케이션을 프로비저닝하고 배포합니다.

    ```bash
    # Azure Developer CLI: 프로비저닝 및 앱 배포
    azd up
    ```

   위 명령어를 실행시킨 후, `AZURE_ENV_NAME` 값을 확인합니다.

4. 애저 포털에서 API 매니지먼트에 접속합니다: `https://apim-<AZURE_ENV_NAME>.azure-api.net`


## 깃헙 코드스페이스 ##

만약, 깃헙 코드스페이스에서 이 리포지토리를 실행시키고 싶다면 아래 명령어를 실행시켜 애저 펑션 앱에서 Swagger UI 페이지가 제대로 작동하도록 합니다.

```bash
# Update local.settings.json
pwsh -c "Invoke-RestMethod https://aka.ms/azfunc-openapi/add-codespaces.ps1 | Invoke-Expression"
```


## 데모 1. APIM 활용 프론트엔드와 백엔드 디커플링 - 네이버 맵 ##

TBD


## 데모 2. APIM 활용 OAuth 인증과정 간소화 - 카카오 프로필 ##

TBD


## 데모 3. APIM 활용 BFF 작성 - 파워 플랫폼 커스텀 커넥터 ##

TBD
