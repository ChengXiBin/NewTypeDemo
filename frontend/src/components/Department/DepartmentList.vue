<template>
  <div class="department-container">
    <div class="header">
      <h2>部門列表</h2>
      <button class="btn btn-primary add-button" @click="handleAddDepartment">
        新增部門
      </button>
    </div>

    <!-- 部門樹狀結構 -->
    <DepartmentTree :departments="treeData" @edit="handleEditDepartment" />

    <!-- 新增/編輯部門 Popup -->
    <DepartmentForm
      v-if="showForm"
      :department="selectedDepartment"
      :departments="allDepartments"
      @submit="handleSaveDepartment"
      @close="showForm = false"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import axios from "axios";
import { API_BASE_URL } from "@/config";
import DepartmentTree from "./DepartmentTree.vue";
import DepartmentForm from "./DepartmentForm.vue";

const departments = ref([]); //從後端取得的「原始」部門資料 (含有 subDepartments)
const treeData =ref([]); //給DepartmentTree 用的「樹狀」資料結構
const allDepartments = ref([]); //給DepartmentForm 用的「所有部門清單」(平列，用來選父部門)

//取得部門清單
const fetchDepartments = async () =>{
  try{
    const response = await axios.get(`${API_BASE_URL}/Department`);
    
    //原始資料
    departments.value = response.data;

    //給樹狀的資料
    treeData.value = buildTree(departments.value);

    //給表單用來選 父部門 的資料
    allDepartments.value = [];
    collectAllDepartments(departments.value,allDepartments.value);

  }catch(error){
    console.log("取得部門清單失敗:", error);
  }
};

//整理 DepartmentTree要用的樹狀結構 
const buildTree = (rawList) => {
  return rawList.map(d=>({
    id: d.departmentID,
    name: d.name,
    children : d.subDepartments &&  d.subDepartments.length >0 ?buildTree(d.subDepartments) :[]
  }))
}

//取得所有不重複部門集合
const collectAllDepartments = (rawList,collector) =>{
  rawList.forEach(d => {
    collector.push({
      id : d.departmentID,
      name : d.name
    })
    if (d.subDepartments && d.subDepartments.length > 0) {
      collectAllDepartments(d.subDepartments, collector)
    }
    console.log("collector:",collector);
  });
}

//元件載入時執行 刷新部門列表資料
onMounted(fetchDepartments)


//事件處理器Event Handler
const showForm = ref(false);
const selectedDepartment = ref(null); //被選擇要編輯的部門資料
const handleAddDepartment = async () => {
  selectedDepartment.value = null;
  showForm.value = true;
};

const handleEditDepartment = async (dept) => {
  selectedDepartment.value ={
    id : dept.departmentID,
    name : dept.name,
    parentId : null
  }
  showForm.value = true;
};

const handleSaveDepartment = async (formData)=> {
  try{
    let message = "";
    //新增部門
    if(formData.id == null){
      await axios.post(`${API_BASE_URL}/Department`,{
        name : formData.name,
        affiliatedDepartmentID : formData.parentId
      })
      message = "新增部門成功";
    }
    else{//編輯部門
      await axios.put(`${API_BASE_URL}/Department`,{
        name : formData.name,
        affiliatedDepartmentID : formData.parentId
      })
      message = "修改部門成功";
    }

    alert(message);
    //成功後重新載入
    await fetchDepartments();
    showForm.value = false;

  }catch(error){
    console.log("儲存部門時發生錯誤",error);
    alert("儲存部門時發生錯誤");
  }
};
</script>

<style scoped>
.department-container {
  padding: 20px;
  background: #ffffff;
  border-radius: 8px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
}

.add-button {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 8px 12px;
  border-radius: 4px;
  cursor: pointer;
}

.add-button:hover {
  background-color: #0056b3;
}
</style>
