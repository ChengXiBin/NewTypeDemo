<template>
  <div class="container mt-5">
    <div class="row justify-content-center">
      <div class="col-md-6 col-lg-4">
        <div class="card p-4 shadow-sm">
          <h1 class="text-center">登入頁面</h1>
          <div class="mb-3">
            <label for="username" class="col-form-label">帳號</label>
            <input
              id="username"
              type="text"
              placeholder="請輸入帳號"
              class="form-control"
              required
              v-model="accountID"
            />
          </div>
          <div class="mb-3">
            <label for="password" class="col-form-label">密碼</label>
            <input
              id="password"
              type="password"
              placeholder="請輸入密碼"
              class="form-control"
              required
              v-model="password"
            />
          </div>
          <button class="btn btn-primary w-100" @click="login">登入</button>
          <p v-if="errorMessage" class="text-danger mt-2">{{ errorMessage }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import { API_BASE_URL } from "../config.js";

const accountID = ref("");
const password = ref("");
const errorMessage = ref("");
const router = useRouter();

const login = async () => {
    try{
        const response = await axios.post(`${API_BASE_URL}/Login`,{
            accountID : accountID.value,
            password : password.value
        });
        //儲存JWT Token 到 localStorage
        localStorage.setItem("token",response.data.token);
        localStorage.setItem("employeeName",response.data.employeeName);
        localStorage.setItem("departmentName",response.data.departmentName);

        router.push("/home");
    }catch(error){
        errorMessage.value = "帳號或密碼錯誤";
    }
};

</script>

<style scoped>
.container {
  margin-top: 100px;
}
.card {
  border-radius: 10px;
}
</style>
