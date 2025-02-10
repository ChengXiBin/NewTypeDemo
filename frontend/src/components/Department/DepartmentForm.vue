<template>
  <div class="modal-overlay">
    <div class="modal-content">
      <h3>{{ isEditMode ? "編輯部門" : "新增部門" }}</h3>

      <div class="form-group">
        <label>部門名稱：</label>
        <input type="text" v-model="localForm.name" />
      </div>

      <div class="form-group">
        <label>上層部門：</label>
        <select v-model="localForm.parentId">
          <!-- 若不選上層部門 -->
          <option :value="null">（無）</option>
          <!-- 從 props.deptOptions 動態生成下拉清單 -->
          <option
            v-for="dept in deptOptions"
            :key="dept.id"
            :value="dept.id"
          >
            {{ dept.name }}
          </option>
        </select>
      </div>

      <div class="modal-actions">
        <button class="btn btn-primary" @click="handleSubmit">
          儲存
        </button>
        <button class="btn btn-secondary" @click="handleClose">
          取消
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
//需要import defineProps, defineEmits  ...  有Bug 沒有import他不會自動偵測到
import { computed, reactive, watchEffect, defineProps, defineEmits } from "vue";  

//接收父層傳來的props
const props = defineProps({
  // 用於表示：當前要編輯的部門，或是 null 代表要新增
  department :{
    type : Object,
    default : null
  },
  // 用於下拉選單顯示全部部門列表 (平列)，以便選擇父部門
  departments :{
    type : Array,
    default : () => []
  }
})

const emit = defineEmits(["submit","close"]);

const localForm = reactive({
  id:null,
  name:"",
  parentId:null,
})

//根據有無Id選擇模式 有ID為編輯模式、無ID為新增模式
const isEditMode = computed(() => !!localForm.id);

watchEffect(() => {
  localForm.id = props.department?.id || null;
  localForm.name = props.department?.name || "";
  localForm.parentId = props.department?.parentId || null;
});

const deptOptions = computed(() => {
  return props.departments.filter(
    (dept) => dept.id !== localForm.id
  );
});

const handleSubmit = () => {
  if(!localForm.name){
    alert("請輸入部門名稱");
    return;
  }

  emit("submit",{
    id : localForm.id,
    name : localForm.name,
    parentId : localForm.parentId,
  });
}

const handleClose = () => {
  emit("close");
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
}

.modal-actions {
  margin-top: 20px;
  display: flex;
  justify-content: space-between;
}

.btn-primary {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 8px 12px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-primary:hover {
  background-color: #0056b3;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
  border: none;
  padding: 8px 12px;
  border-radius: 4px;
  cursor: pointer;
}

.btn-secondary:hover {
  background-color: #5a6268;
}
</style>
