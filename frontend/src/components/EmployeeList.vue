<template>
    <div class="container mt-5">
        <div class="d-flex justify-content-between align-items-center mb3">
            <h2>人員列表</h2>
            <button class="btn btn-primary" @click="showAddPopup=true">新增人員</button>
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
                <tr v-for="employee in  employees" :key="employee.id">
                    <td>{{ employee.name }}</td>
                    <td>{{ employee.department }}</td>
                    <td>
                        <button class="btn btn-sm btn-warning me-2" @click="editEmployee(employee)">編輯</button>
                    </td>
                </tr>
            </tbody>
        </table>

        <!--新增人員Popup-->
        <div class="modal-overlay" v-if="showAddPopup">
            <div class="modal-content">
                <h4>新增人員</h4>
                <input type="text" class="form-control mb-2" v-model="newEmployee.name" placeholder="姓名">
                <select class="form-select mb-2" v-model="newEmployee.department">
                    <option value="" disabled>請選擇部門</option>
                    <option v-for="dept in departments" :key="dept" :value="dept">{{ dept }}</option>
                </select>
                <input type="text" class="form-control mb-2" v-model="newEmployee.account" placeholder="帳號">
                <input type="text" class="form-control mb-2" v-model="newEmployee.password" placeholder="密碼">
                <input type="text" class="form-control mb-2" v-model="newEmployee.email" placeholder="信箱">
                <button class="btn btn-primary w-100" @click="addEmployee">儲存</button>
                <button class="btn btn-secondary w-100 mt-2" @click="showAddPopup=false">取消</button>
            </div>
        </div>

        <!--編輯人員Popup-->
        <div class="modal-overlay" v-if="showEditPopup">
            <div class="modal-content">
                <h4>編輯人員</h4>
                <input type="text" class="form-control mb-2" v-model="selectedEmployee.name" placeholder="姓名">
                <select class="form-select mb-2" v-model="selectedEmployee.department">
                    <option v-for="dept in departments" :key="dept" :value="dept">{{ dept }}</option>
                </select>
                <button class="btn btn-primary w-100" @click="updateEmployee">更新</button>
                <button class="btn btn-secondary w-100 mt-2" @click="showEditPopup=false">取消</button>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    name:"EmployeeForm",
    data(){
        return{
            employees:[
                {id:1, name:"王小明", department:"人事部"},
                {id:1, name:"小叮噹", department:"財務部"},
            ],
            showAddPopup:false,
            showEditPopup:false,
            newEmployee:{nae:"", department:"", account:"", password:"" ,email:""},
            selectedEmployee : null,
            departments:["人事部", "財務部", "工程部"]
        };
    },
    methods:{
        addEmployee(){
            if(this.newEmployee.name){
                const newId=this.employees.length + 1;
                
                //   ...this.newEmployee 語法  
                //   ...是複製this.newEmployee新的物件 不影響舊this.newEmployee資料
                this.employees.push({id:newId, ...this.newEmployee}); 
                
                this.newEmployee = {name:"", department:"", account:"", password:"", email:""};
                this.showAddPopup = false;
            }else{
                alert("請輸入姓名、部門、帳號、密碼、信箱");
            }
        },
        editEmployee(employee){
            this.selectedEmployee = {...employee};
            this.showEditPopup = true;
        },
        updateEmployee(){
            const index = this.employees.findIndex(emp=>emp.id === this.selectedEmployee.id);
            if(index != -11){
                this.employees.splice(index, 1 , this.selectedEmployee);
                this.showEditPopup = false;
            }
        }
    },
}
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
