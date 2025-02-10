<template>
  <div class="container mt-5">
    <div class="d-flex justify-content-between align-items-center mb3">
      <h2>人員列表</h2>
      <button class="btn btn-primary" @click="showAddPopup = true">
        新增人員
      </button>
    </div>

    <!--人員列表-->
    <table class="table table-bordered">
      <thead class="table-light">
        <tr>
          <th>姓名</th>
          <th>部門</th>
          <th>操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="employee in employees" :key="employee.id">
          <td>{{ employee.name }}</td>
          <td>{{ employee.department }}</td>
          <td>
            <button
              class="btn btn-sm btn-warning me-2"
              @click="editEmployee(employee)"
            >
              編輯
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <!--新增人員Popup-->
    <div class="modal-overlay" v-if="showAddPopup">
      <div class="modal-content">
        <h4>新增人員</h4>
        <input
          type="text"
          class="form-control mb-2"
          v-model="newEmployee.name"
          placeholder="姓名"
        />
        <select class="form-select mb-2" v-model="newEmployee.department">
          <option value="" disabled>請選擇部門</option>
          <option v-for="dept in departments" :key="dept" :value="dept">
            {{ dept }}
          </option>
        </select>
        <input
          type="text"
          class="form-control mb-2"
          v-model="newEmployee.account"
          placeholder="帳號"
        />
        <input
          type="text"
          class="form-control mb-2"
          v-model="newEmployee.password"
          placeholder="密碼"
        />
        <input
          type="text"
          class="form-control mb-2"
          v-model="newEmployee.email"
          placeholder="信箱"
        />
        <button class="btn btn-primary w-100" @click="addEmployee">儲存</button>
        <button
          class="btn btn-secondary w-100 mt-2"
          @click="showAddPopup = false"
        >
          取消
        </button>
      </div>
    </div>

    <!--編輯人員Popup-->
    <div class="modal-overlay" v-if="showEditPopup">
      <div class="modal-content">
        <h4>編輯人員</h4>
        <input
          type="text"
          class="form-control mb-2"
          v-model="selectedEmployee.name"
          placeholder="姓名"
        />
        <select class="form-select mb-2" v-model="selectedEmployee.department">
          <option v-for="dept in departments" :key="dept" :value="dept">
            {{ dept }}
          </option>
        </select>
        <button class="btn btn-primary w-100" @click="updateEmployee">
          更新
        </button>
        <button
          class="btn btn-secondary w-100 mt-2"
          @click="showEditPopup = false"
        >
          取消
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import {ref,onMounted} from "vue";
import axios from "axios";
import {API_BASE_URL} from "../config.js";

const employees = ref([]);
const showAddPopup = ref(false);
const showEditPopup = ref(false);
const newEmployee = ref({
        AccountID: "",
        Password: "",
        DisplayName: "",
        Email: "",
        DepartmentIDs: [],
      });
const selectedEmployee = ref(null);
const departments = ref(null);

const getDepartments = async ()=>{
  try{
    const response = await axios.get(`${API_BASE_URL}/Department`);
    departments.value = response.data;
  }catch(error){
    console.error("取得部門列表失敗",error);
}};

//取得員工列表
const getEmployees = async ()=>{
  try{
    const response = await axios.get(`${API_BASE_URL}/Employee`);
    employees.value = response.data;
  }catch(error){
    console.error("取得員工列表失敗",error);
}};

const addEmployee = async ()=> {
  try{
   const response = await axios.post(`${API_BASE_URL}/Employee`,newEmployee.value);
   console.log(`新增員工訊息: ${response.data}`);
   showAddPopup.value = false;
   newEmployee.value = { AccountID:"", Password:"",DisplayName:"",Email:"",DepartmentIDs:[]};
   getEmployees();
  }catch(error){
    console.error("新增員工失敗",error);
}};

const updateEmployee = async () => {
      try{

        getEmployees();
      }catch(error){
        console.error("員工失敗",error);
      }
    };

onMounted(getEmployees,getDepartments);
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  width: 300px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.3);
}
</style>
