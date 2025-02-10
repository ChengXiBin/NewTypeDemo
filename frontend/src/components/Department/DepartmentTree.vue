<template>
  <ul class="department-tree">
    <li v-for="dept in departments" :key="dept.id">
      <div class="department-item">
        <!-- 展開/收合符號 + 部門名稱 -->
        <span @click="toggleExpand(dept.id)">
          {{ isExpanded[dept.id] ? "▼" : "▶" }} {{ dept.name }}
        </span>

        <!-- 編輯按鈕 -->
        <button
          class="btn btn-sm btn-warning ms-2"
          @click="$emit('edit', dept)"
        >
          編輯
        </button>
      </div>

      <!-- 遞迴顯示子部門 -->
      <DepartmentTree
        v-if="dept.children && dept.children.length > 0 && isExpanded[dept.id]"
        :departments="dept.children"
        @edit="$emit('edit', $event)"
        class="child-department"
      />
    </li>
  </ul>
</template>

<script setup>
import { ref, defineProps} from "vue";

// 接收父層傳入的 props：樹狀部門陣列
defineProps({
  departments: {
    type: Array,
    default: () => []
  }
});

// 記錄展開/收合的狀態：{ [deptId]: true/false }
const isExpanded = ref({});

// 點擊名稱時，切換展開 / 收合
function toggleExpand(id) {
  isExpanded.value[id] = !isExpanded.value[id];
}
</script>

<style scoped>
.department-tree {
  list-style-type: none;
  padding-left: 10px;
  margin: 0;
}

.department-tree li {
  margin-left: 20px;
  padding: 5px 0;
}

.department-item {
  display: flex;
  align-items: center;
  cursor: pointer;
}

.child-department {
  margin-left: 20px;
}
</style>
