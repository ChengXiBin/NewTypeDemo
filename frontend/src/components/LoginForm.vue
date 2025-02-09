<template>
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6 col-lg-4">
                <div class="card p-4 shadow-sm">
                    <h1 class="text-center">登入頁面</h1>
                    <div class="mb-3">
                        <label for="username" class="col-form-label">帳號</label>
                        <input id="username" type ="text" placeholder="請輸入帳號" class="form-control" />
                    </div>
                    <div class="mb-3">
                        <label for="password" class="col-form-label">密碼</label>
                        <input id="password" type="password" placeholder="請輸入密碼" class="form-control"/>
                    </div>
                    <button class="btn btn-primary w-100" @click="handleLogin">登入</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'LoginForm',
        methods: {
            handleLogin() {
                //登入轉導判斷邏輯 待補
                this.$router.push('/home');
            }
        }
    };
</script>