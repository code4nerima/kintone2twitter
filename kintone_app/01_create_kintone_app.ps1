# 環境設定
$sub_domain='xxxx';
$user_account='xxxx';
$password='xxxx';
$app_name='xxxx';

$create_app_api_url='https://(sub-domain).cybozu.com/k/v1/preview/app.json'.Replace('(sub-domain)',$sub_domain);
$create_field_api_url='https://(sub-domain).cybozu.com/k/v1/preview/app/form/fields.json'.Replace('(sub-domain)',$sub_domain);

# kintoneにユーザ認証でログインするため、「ログイン名:パスワード」をBASE64エンコード
# https://developer.cybozu.io/hc/ja/articles/201941754#step7
$authorization_string = [Convert]::ToBase64String(([System.Text.Encoding]::Default).GetBytes($user_account + ':' + $password));

# リクエストヘッダに指定
$headers = @{
    'X-Cybozu-Authorization' = $authorization_string;
    'Content-Type' = 'application/json';
}

# bodyの指定
$create_app_body = @{
    name = $app_name
}
# jsonオブジェクトをバイト配列に変換
$create_app_request_body = [System.Text.Encoding]::UTF8.GetBytes(($create_app_body | ConvertTo-Json))

# アプリの作成apiの実行
$responce= Invoke-RestMethod -Method 'Post' -Uri $create_app_api_url -Headers $headers -Body $create_app_request_body -ContentType 'application/json'
$app_id = $responce.app;

############################################################
# フィールドの追加

# bodyの指定（テンプレート文字列に、@app_idを指定)
$create_field_app_body = (Get-Content -LiteralPath .\create_field_template.json -Encoding UTF8 -Raw).Replace('@app_id',$app_id);
$create_field_request_body = [System.Text.Encoding]::UTF8.GetBytes($create_field_app_body)

# フィールドの追加apiの実行
Invoke-RestMethod -Method 'Post' -Uri $create_field_api_url -Headers $headers -Body $create_field_request_body -ContentType 'application/json'
