<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-light">
    <div class="container-fluid">
      <a class="navbar-brand" href="#">MyApp</a>
      <div class="collapse navbar-collapse">
        <ul class="navbar-nav">
          <li class="nav-item">
            <router-link to="/home" class="nav-link">首頁</router-link>
          </li>
          <li class="nav-item">
            <router-link to="/employee" class="nav-link">人員列表</router-link>
          </li>
          <li class="nav-item">
            <router-link to="/department" class="nav-link"
              >部門列表</router-link
            >
          </li>
        </ul>
      </div>
      <div class="d-flex align-items-center">
        <span class="navbar-text me-3 fw-bold">👤{{ employeeName }} - {{ departmentName }}</span>
        <button class="btn btn-outline-danger btn-sm" @click="logout">登出</button>
      </div>
    </div>
  </nav>
</template>

<script setup>
import {ref,onMounted} from "vue";
import {useRouter} from "vue-router";

const employeeName = ref("");
const departmentName = ref("");
const router = useRouter();

//元件載入時觸發
onMounted(()=>{
  employeeName.value = localStorage.getItem("employeeName");
  departmentName.value = localStorage.getItem("departmentName");
})

const logout = () => {
      localStorage.removeItem("token");
      localStorage.removeItem("employeeName");
      localStorage.removeItem("departmentName");
      router.push("/login");
    }
</script>
